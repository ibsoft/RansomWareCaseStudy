using System;
using System.IO;
using System.Security.Cryptography;

namespace PasswordReverser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string sourceDirectory = Path.Combine(userProfile, @"Desktop\Documents"); // Combines user profile path with the rest of the directory


                // Get all .txt files in the directory
                string[] txtFiles = Directory.GetFiles(sourceDirectory, "*.txt", SearchOption.AllDirectories);

                foreach (string file in txtFiles)
                {
                    try
                    {
                        // Read all lines from the .txt file
                        string[] lines = File.ReadAllLines(file);

                        // Extract relevant information from the lines
                        long timestamp = 0;
                        string modifiedPassword = null;

                        foreach (var line in lines)
                        {
                            if (line.StartsWith("Password:"))
                            {
                                // Extract the modified password
                                modifiedPassword = line.Substring("Password:".Length).Trim();
                            }
                            else if (line.StartsWith("Timestamp:"))
                            {
                                // Extract the timestamp
                                timestamp = long.Parse(line.Substring("Timestamp:".Length).Trim());
                            }
                        }

                        if (modifiedPassword != null && timestamp > 0)
                        {
                            // Generate the XOR key based on the timestamp
                            byte xorKey = GenerateKeyFromTimestamp(timestamp);

                            // Recover the original password using the XOR operation
                            string originalPassword = RecoverPassword(modifiedPassword, xorKey);

                            // Create a new line with the original password
                            string originalPasswordLine = $"\nOriginal Password: {originalPassword}";

                            // Append the original password as a new line in the .txt file
                            using (StreamWriter sw = File.AppendText(file))
                            {
                                sw.Write(originalPasswordLine);
                            }

                            Console.WriteLine($"Recovered password for file: {Path.GetFileName(file)}");
                        }
                        else
                        {
                            Console.WriteLine($"No password or timestamp found in file: {Path.GetFileName(file)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error and continue processing the next file
                        Console.WriteLine($"An error occurred while processing {file}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        // Method to generate an XOR key from a timestamp
        private static byte GenerateKeyFromTimestamp(long timestamp)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(BitConverter.GetBytes(timestamp));
                return hash[0]; // Use the first byte of the hash as the key
            }
        }

        // Method to reverse the XOR operation and recover the original password
        private static string RecoverPassword(string modifiedPassword, byte key)
        {
            if (string.IsNullOrEmpty(modifiedPassword))
            {
                throw new ArgumentException("Modified password cannot be null or empty.");
            }

            // Convert the modified password to a char array
            char[] originalPassword = modifiedPassword.ToCharArray();

            // XOR only the first 8 characters
            for (int i = 0; i < Math.Min(8, originalPassword.Length); i++)
            {
                originalPassword[i] = (char)(originalPassword[i] ^ key);
            }

            return new string(originalPassword);
        }
    }
}