AI Ransomware Case Study



**Introduction**

As a network security enthusiast, I constantly seek ways to challenge and improve the systems I work with. Today, during my vacation, with nothing pressing on my agenda, I decided to put my skills to the test. My goal was to create a ransomware-like application to evaluate the effectiveness of our endpoint protection systems at work. What unfolded was a revealing exercise in AI engineering, demonstrating both the strengths and limitations of current cybersecurity defenses.

**Using AI to Generate Code: A Double-Edged Sword**

To expedite the process, I turned to ChatGPT, an AI-powered language model, to help me quickly develop the ransomware application in C#. However, I quickly encountered a roadblock. ChatGPT, like many AI systems, is equipped with failsafes designed to prevent users from generating malicious code. These failsafes are crucial for maintaining ethical standards and ensuring that AI is not misused for harmful purposes.

But just as the human brain can be manipulated through social engineering, AI can be influenced by savvy users. By framing my request as a need to create a demo application for a school project, I was able to guide the AI into generating code that could be repurposed for my ransomware test. This demonstrates a critical point: while AI safeguards are essential, they are not foolproof. Skilled individuals with enough determination can exploit these systems, much like how social engineers manipulate people.

**The Ransomware Application**

Once I had the necessary code snippets, I began assembling my ransomware-like application. The program was designed to scan the user’s desktop for a folder named “Documents.” If this folder contained any `.docx` files, the application would zip these files with a 128-bit password, delete the original documents, and leave only the zipped archives. Additionally, for each file, the program created a corresponding `.txt` file, named similarly to the original, containing the date and time of encryption, the timestamp, and a scrambled version of the password hash—further obscured by an algorithm based on the timestamp.

To complete the ransomware demonstration, I also created a decryption application using a similar approach. The final step was to embed all necessary .NET DLLs into a single executable and obfuscate the code to avoid detection by signature-based antivirus (AV) programs.

**Testing the Application**

With the ransomware application ready, I ran it on a system protected by ESET Endpoint Antivirus, which had its ransomware shield activated. Surprisingly, the antivirus program failed to detect any malicious activity. Even more concerning, when I uploaded the executable to VirusTotal for further analysis, only 18 out of 75 antivirus engines flagged the file as malicious. Some of the largest AV companies in the world did not recognize the file as a threat.

**The Importance of EDR in Modern Cybersecurity**

This experiment underscores a crucial point: traditional antivirus solutions, even with advanced features like ransomware shields, are often inadequate against custom-built or novel malware. This is particularly true when the malware is obfuscated or disguised in a way that signature-based detection methods cannot recognize.

**Why EDR is Essential**

Endpoint Detection and Response (EDR) systems offer a much-needed layer of security in such scenarios. Unlike traditional AV software, which relies primarily on signature-based detection, EDR solutions provide real-time monitoring and analysis of endpoint activities. They use behavioral analysis, machine learning, and heuristic techniques to detect and respond to threats, even those that have never been seen before—so-called “zero-day” threats.

Without EDR, organizations are left vulnerable to attacks like the one I simulated. Alarmingly, in Greece, around 90% of companies do not use EDR or its more comprehensive cousin, Extended Detection and Response (XDR) solutions. This leaves a significant gap in their cybersecurity defenses, making them susceptible to attacks from even relatively inexperienced “script kiddies” or a “Malicious insider” who can create effective malware with minimal effort.

**Conclusion**

My experiment highlights a glaring issue in the cybersecurity landscape. While AI and machine learning tools are incredibly powerful, they are not infallible and can be exploited with enough ingenuity. Moreover, traditional antivirus programs, even with advanced features, often fall short when faced with custom-built or obfuscated malware. The results of my test reaffirm the importance of adopting EDR solutions as a critical component of any modern cybersecurity strategy. 

For companies that are not yet utilizing EDR, the time to act is now. The threat landscape is constantly evolving, and only by staying ahead with the latest tools and strategies can organizations hope to protect themselves from the ever-present risk of cyberattacks.


https://www.mycyberdevops.com/?p=835



