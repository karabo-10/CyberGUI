using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace PROG_Part2_C_
{
    public partial class Form1 : Form
    {
        // ─── Memory & State ───────────────────────────────────────────────
        private string userName = "";
        private string favouriteTopic = "";
        private string lastTopic = "";
        private bool waitingForName = true;
        private bool isTyping = false; // prevent double sends while typing

        // ─── Keyword Responses ─────────────────────────────────────────────
        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["password"] = new List<string>
            {
                "Please make sure you use a strong password with a mix of letters, numbers, and symbols. Thank you for taking your security seriously!",
                "Never reuse the same password across multiple sites. Consider using a password manager like Bitwarden.",
                "Enable two-factor authentication (2FA) alongside a strong password for maximum account security.",
                "As you create passwords, please follow the company's policies and guidelines which guide you in creating secure passwords."


            },
            ["phishing"] = new List<string>
            {
                "Phishing is when attackers trick you into revealing sensitive information through fake emails or messages. Always check the sender's address and avoid clicking suspicious links!",
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Never click links in unexpected emails. Go directly to the official website by typing the address yourself."
            },
            ["browsing"] = new List<string>
            {
                "Always use secure websites (https) and avoid clicking unknown links. Thank you for browsing safely!",
                "Consider using a browser extension like uBlock Origin to block malicious ads and trackers.",
                "Keep your browser updated — updates often include important security patches."
            },
            ["privacy"] = new List<string>
            {
                "Protect your privacy by limiting what you share online and reviewing your app permissions regularly.",
                "Review your social media privacy settings regularly to control who sees your information.",
                "Be mindful of what personal data you share online — once it's out there, it's hard to take back."
            },
            ["malware"] = new List<string>
            {
                "Malware is harmful software that can damage your device or steal your data. Avoid downloading files from unknown sources!",
                "Keep your antivirus software up to date to defend against the latest malware threats.",
                "Regularly scan your device and be cautious of unexpected pop-ups asking you to install something."
            },
            ["scam"] = new List<string>
            {
                "Online scams try to trick you into giving away money or personal information. Always verify messages before trusting them!",
                "If an offer seems too good to be true, it probably is. Always verify before sharing any details.",
                "Scammers often create urgency — slow down and verify any unexpected requests for money or info."
            },
            ["wifi"] = new List<string>
            {
                "Be careful when using public Wi-Fi. Avoid logging into sensitive accounts like banking on public networks!",
                "Use a VPN on public Wi-Fi to protect your browsing data from prying eyes.",
                "If possible, use your mobile data instead of public Wi-Fi for sensitive transactions."
            },
            ["virus"] = new List<string>
            {
                "Computer viruses are malicious programs that can harm your system. Use antivirus software and avoid unknown files!",
                "Keep your operating system updated — many viruses exploit outdated software vulnerabilities.",
                "Never open email attachments from unknown senders, even if they look legitimate."
            }
        };

        // ─── Sentiment Detection ───────────────────────────────────────────
        private Dictionary<string, string> sentimentPrefixes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["worried"]     = "It's completely understandable to feel that way. Let me share some tips to help you stay safe. ",
            ["scared"]      = "Don't worry — you're already doing the right thing by learning about this. ",
            ["frustrated"]  = "I hear you — cybersecurity can feel overwhelming. Let's break it down step by step. ",
            ["confused"]    = "No problem at all! Let me explain that more clearly for you. ",
            ["curious"]     = "Great curiosity! Staying informed is the best defence. Here's something useful: ",
            ["anxious"]     = "Take a deep breath — awareness is the first step to staying safe online. ",
            ["angry"]       = "I understand your frustration. Let's channel that energy into staying better protected. ",
            ["overwhelmed"] = "Let's take it one step at a time. Here's one simple thing you can do: "
        };

        // ─── Follow-up phrases ─────────────────────────────────────────────
        private List<string> followUpPhrases = new List<string>
        {
            "tell me more", "another tip", "give me more", "explain more",
            "more info", "elaborate", "continue", "more please", "go on"
        };

        private Random rng = new Random();

        // ══════════════════════════════════════════════════════════════════
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        // ─── On Form Load ──────────────────────────────────────────────────
        private void Form1_Load(object sender, EventArgs e)
        {
            PlayVoiceGreeting();
            ShowAsciiLogo();
            AppendDivider();
            AppendSection("Welcome");
            AppendBot("Hello! I am your CyberSecurity Awareness Bot. 🛡");
            AppendBot("Ask me about password security, phishing, or safe browsing.");
            AppendDivider();
            AppendBot("Please enter your name to get started:");
        }

        // ─── Send / Enter ──────────────────────────────────────────────────
        private void btnSend_Click(object sender, EventArgs e) => ProcessInput();

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; ProcessInput(); }
        }

        private void ProcessInput()
        {
            // Block new input while typing animation is running
            if (isTyping) return;

            string input = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AppendUser(input);
            txtInput.Clear();

            // ── Collect and validate name ──────────────────────────────────
            if (waitingForName)
            {
                if (!IsValidName(input))
                {
                    AppendError("Please enter a valid name using letters only (no numbers or symbols).");
                    return;
                }
                userName = input;
                waitingForName = false;

                AppendDivider();
                AppendSection("Welcome");
                // Use TypeResponse ONCE for the welcome message
                TypeResponse($"Welcome {userName}! I am your CyberSecurity Awareness Bot.\nAsk me about password security, phishing, or safe browsing.");
                return;
            }

            // ── Exit ───────────────────────────────────────────────────────
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                AppendDivider();
                AppendColoured($"Goodbye {userName}! Stay safe online. 👋", Color.FromArgb(255, 220, 50));
                AppendDivider();
                btnSend.Enabled  = false;
                txtInput.Enabled = false;
                return;
            }

            // ── Build and type response ────────────────────────────────────
            string response = BuildResponse(input.ToLower());
            TypeResponse(response);
        }

        // ─── Core Response Builder ─────────────────────────────────────────
        private string BuildResponse(string lower)
        {
            string sentimentPrefix = "";
            foreach (var kv in sentimentPrefixes)
                if (lower.Contains(kv.Key)) { sentimentPrefix = kv.Value; break; }

            // Follow-up
            foreach (var phrase in followUpPhrases)
            {
                if (lower.Contains(phrase) && !string.IsNullOrEmpty(lastTopic)
                    && keywordResponses.ContainsKey(lastTopic))
                    return sentimentPrefix + keywordResponses[lastTopic][rng.Next(keywordResponses[lastTopic].Count)];
            }

            // Memory recall
            if (lower.Contains("what do you remember") || lower.Contains("what do you know about me"))
            {
                string mem = $"I remember your name is {userName}.";
                if (!string.IsNullOrEmpty(favouriteTopic))
                    mem += $" You mentioned you're interested in {favouriteTopic}.";
                return mem;
            }

            if (lower.Contains("how are you"))
                return $"I'm functioning perfectly, thank you for asking, {userName}! I'm here and ready to help you stay safe online.";

            if (lower.Contains("purpose"))
                return $"My purpose is to help you, {userName}, learn about cybersecurity and safe online practices.";

            if (lower.Contains("what can i ask") || lower.Contains("help") || lower.Contains("topics"))
                return $"{userName}, you can ask me about passwords, phishing, safe browsing, malware, scams, wifi safety, and viruses!";

            if (lower.Contains("hello") || lower.Contains("hi") || lower.Contains("hey"))
                return $"Hello {userName}! How can I help you stay safe online today?";

            // Keyword matching
            foreach (var kv in keywordResponses)
            {
                if (lower.Contains(kv.Key))
                {
                    lastTopic = kv.Key;

                    if (lower.Contains("interest") || lower.Contains("love") || lower.Contains("like") || lower.Contains("favourite"))
                    {
                        favouriteTopic = kv.Key;
                        return $"Great! I'll remember that you're interested in {kv.Key}. It's a crucial part of staying safe online.\n"
                             + kv.Value[rng.Next(kv.Value.Count)];
                    }

                    string personalised = (!string.IsNullOrEmpty(favouriteTopic)
                        && favouriteTopic.Equals(kv.Key, StringComparison.OrdinalIgnoreCase))
                        ? $"As someone interested in {favouriteTopic}, here's a tip: " : "";

                    return sentimentPrefix + personalised + $"{userName}, " + kv.Value[rng.Next(kv.Value.Count)];
                }
            }

            return $"Sorry {userName}, I did not quite understand that. Could you please rephrase? Try asking about passwords, phishing, scams, privacy, or malware.";
        }

        // ─── Name validation ───────────────────────────────────────────────
        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            foreach (char c in name)
                if (!char.IsLetter(c)) return false;
            return true;
        }

        // ─── FIXED Typing animation — runs on background thread safely ─────
        private void TypeResponse(string message)
        {
            isTyping = true;
            btnSend.Enabled  = false;
            txtInput.Enabled = false;

            var thread = new Thread(() =>
            {
                // Print label ONCE
                rtbChat.Invoke((Action)(() =>
                {
                    int s = rtbChat.TextLength;
                    rtbChat.AppendText("[CyberBot]: ");
                    rtbChat.Select(s, 12);
                    rtbChat.SelectionColor = Color.FromArgb(0, 200, 150);
                    rtbChat.SelectionFont  = new Font("Consolas", 10F, FontStyle.Bold);
                    rtbChat.SelectionLength = 0;
                }));

                // Type each character with delay
                foreach (char c in message)
                {
                    char ch = c;
                    rtbChat.Invoke((Action)(() =>
                    {
                        int pos = rtbChat.TextLength;
                        rtbChat.AppendText(ch.ToString());
                        rtbChat.Select(pos, 1);
                        rtbChat.SelectionColor = Color.FromArgb(200, 220, 255);
                        rtbChat.SelectionFont  = new Font("Consolas", 10F);
                        // Move cursor to end — this auto-scrolls without shaking
                        rtbChat.SelectionStart  = rtbChat.TextLength;
                        rtbChat.SelectionLength = 0;
                    }));

                    if (c == '.' || c == '!' || c == '?')
                        Thread.Sleep(150);
                    else
                        Thread.Sleep(20);
                }

                // Add newlines and re-enable input
                rtbChat.Invoke((Action)(() =>
                {
                    rtbChat.AppendText("\n\n");
                    rtbChat.ScrollToCaret();
                    isTyping         = false;
                    btnSend.Enabled  = true;
                    txtInput.Enabled = true;
                    txtInput.Focus();
                }));
            });

            thread.IsBackground = true;
            thread.Start();
        }

        // ─── ASCII Logo ────────────────────────────────────────────────────
        private void ShowAsciiLogo()
        {
            string logo = @"
>>====================================================================<<
||              _                                        _ _          ||
||    ___ _   _| |__   ___ _ __ ___  ___  ___ _   _ _ __(_) |_ _   _  ||
||   / __| | | | '_ \ / _ \ '__/ __|/ _ \/ __| | | | '__| | __| | | | ||
||  | (__| |_| | |_) |  __/ |  \__ \  __/ (__| |_| | |  | | |_| |_| | ||
||   \___|\__, |_.__/ \___|_|  |___/\___|\___|\__,_|_|  |_|\__|\__, | ||
||        |___/                                       _        |___/  ||
||  __ ___      ____ _ _ __ ___ _ __   ___  ___ ___  | |__   ___ | |_ ||
|| / _` \ \ /\ / / _` | '__/ _ \ '_ \ / _ \/ __/ __| | '_ \ / _ \| __|||
||| (_| |\ V  V / (_| | | |  __/ | | |  __/\__ \__ \ | |_) | (_) | |_ ||
|| \__,_| \_/\_/ \__,_|_|  \___|_| |_|\___||___/___/ |_.__/ \___/ \__|||
>>====================================================================<<
                        /-----\
                       | 0   0 |
                       |   ^   |
                       |  ---  |
                       |_______|
                        /| | |\
                       /_|_|_|_\
                    Stay Cyber Safe
";
            int start = rtbChat.TextLength;
            rtbChat.AppendText(logo + "\n");
            rtbChat.Select(start, logo.Length);
            rtbChat.SelectionColor = Color.FromArgb(0, 210, 210);
            rtbChat.SelectionFont  = new Font("Consolas", 9F, FontStyle.Bold);
            rtbChat.ScrollToCaret();
        }

        // ─── Section header ────────────────────────────────────────────────
        private void AppendSection(string title)
        {
            int start = rtbChat.TextLength;
            string text = $"\n=== {title} ===\n";
            rtbChat.AppendText(text);
            rtbChat.Select(start, text.Length);
            rtbChat.SelectionColor = Color.FromArgb(220, 100, 255);
            rtbChat.SelectionFont  = new Font("Consolas", 11F, FontStyle.Bold);
        }

        // ─── Divider ───────────────────────────────────────────────────────
        private void AppendDivider()
        {
            int start = rtbChat.TextLength;
            string line = "\n" + new string('=', 70) + "\n";
            rtbChat.AppendText(line);
            rtbChat.Select(start, line.Length);
            rtbChat.SelectionColor = Color.FromArgb(80, 80, 80);
        }

        // ─── AppendBot (instant, for startup messages only) ────────────────
        private void AppendBot(string message)
        {
            int s = rtbChat.TextLength;
            rtbChat.AppendText("[CyberBot]: ");
            rtbChat.Select(s, 12);
            rtbChat.SelectionColor = Color.FromArgb(0, 200, 150);
            rtbChat.SelectionFont  = new Font("Consolas", 10F, FontStyle.Bold);

            int ms = rtbChat.TextLength;
            rtbChat.AppendText(message + "\n\n");
            rtbChat.Select(ms, message.Length);
            rtbChat.SelectionColor = Color.FromArgb(200, 220, 255);
            rtbChat.SelectionFont  = new Font("Consolas", 10F);
            rtbChat.ScrollToCaret();
        }

        // ─── AppendUser ────────────────────────────────────────────────────
        private void AppendUser(string message)
        {
            string label = string.IsNullOrEmpty(userName) ? "[You]: " : $"[{userName}]: ";
            int s = rtbChat.TextLength;
            rtbChat.AppendText(label);
            rtbChat.Select(s, label.Length);
            rtbChat.SelectionColor = Color.FromArgb(100, 200, 100);
            rtbChat.SelectionFont  = new Font("Consolas", 10F, FontStyle.Bold);

            int ms = rtbChat.TextLength;
            rtbChat.AppendText(message + "\n");
            rtbChat.Select(ms, message.Length);
            rtbChat.SelectionColor = Color.White;
            rtbChat.SelectionFont  = new Font("Consolas", 10F);
            rtbChat.ScrollToCaret();
        }

        // ─── AppendError 
        private void AppendError(string message)
        {
            int s = rtbChat.TextLength;
            rtbChat.AppendText(message + "\n\n");
            rtbChat.Select(s, message.Length);
            rtbChat.SelectionColor = Color.FromArgb(255, 80, 80);
            rtbChat.SelectionFont  = new Font("Consolas", 10F);
        }

        // ─── AppendColoured ────────────────────────────────────────────────
        private void AppendColoured(string message, Color color)
        {
            int s = rtbChat.TextLength;
            rtbChat.AppendText(message + "\n\n");
            rtbChat.Select(s, message.Length);
            rtbChat.SelectionColor = color;
            rtbChat.SelectionFont  = new Font("Consolas", 10F, FontStyle.Bold);
        }

        // ─── Voice Greeting ────────────────────────────────────────────────
        private void PlayVoiceGreeting()
        {
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    SoundPlayer player = new SoundPlayer("greeting.wav");
                    player.Play();
                }
                catch { }
            }
        }

        // ─── Clear button ──────────────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbChat.Clear();
            ShowAsciiLogo();
            AppendBot($"Chat cleared! How can I help you, {userName}?");
        }
    }
}
