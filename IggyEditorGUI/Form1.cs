using System.Text;
using System.Windows.Forms;

namespace IggyEditorGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RedirectConsoleOutput();
            iggy = new Iggy();
        }

        private Iggy iggy;
        public string iggyPath;
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Iggy Files|*.iggy";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    iggyPath = openFileDialog.FileName;
                    byte[] iggyData = File.ReadAllBytes(openFileDialog.FileName);
                    if (iggy.Load(iggyData))
                    {
                        // Display information about the loaded Iggy file in richTextBox1
                        iggyInfo.Text = $"Signature: 0x{iggy.header.signature:X}\n";
                        iggyInfo.Text += $"Version: {iggy.header.version}\n";
                        iggyInfo.Text += $"Platform: {BitConverter.ToString(iggy.header.platform)}\n";
                        iggyInfo.Text += $"Unknown Field (unk_0C): {iggy.unk_0C}\n";
                        iggyInfo.Text += $"Number of Subfiles: {iggy.header.num_subfiles}\n";
                        iggyInfo.Text += $"Subfiles:\n";

                        for (int i = 0; i < iggy.header.num_subfiles; i++)
                        {
                            iggyInfo.Text += $"Subfile {i + 1}:\n";
                            iggyInfo.Text += $"ID: {iggy.subFileEntries[i].id}\n";
                            iggyInfo.Text += $"Size: {iggy.subFileEntries[i].size}\n";
                            iggyInfo.Text += $"Size2: {iggy.subFileEntries[i].size2}\n";
                            iggyInfo.Text += $"Offset: {iggy.subFileEntries[i].offset}\n";
                            iggyInfo.Text += "\n";
                        }
                        // Update TextBox controls with current dimensions from iggy
                        tbHeight.Text = $"{iggy.header.height}x{iggy.header.width}";
                        tbWidth.Text = $"{iggy.header.width}x{iggy.header.height}";

                        MessageBox.Show("File loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    else
                    {
                        MessageBox.Show("Failed to load the Iggy file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveWithNewDimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tbWidth.Text.Trim() != "" && tbHeight.Text.Trim() != "")
            {
                int newWidth, newHeight;
                if (int.TryParse(tbWidth.Text, out newWidth) && int.TryParse(tbHeight.Text, out newHeight))
                {
                    // Modify Iggy dimensions and save the modified file
                    iggy.ModifyImageDimensions(newWidth, newHeight);
                    SaveModifiedIggyFile(iggyPath);
                }
                else
                {
                    MessageBox.Show("Invalid width or height values. Please enter numeric values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter both width and height values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveModifiedIggyFile(string outputPath)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
                {
                    // Save the modified header
                    byte[] headerBytes = iggy.StructureToByteArray(iggy.header);
                    writer.Write(headerBytes);

                    // Save the modified subfile entries
                    foreach (var entry in iggy.subFileEntries)
                    {
                        byte[] entryBytes = iggy.StructureToByteArray(entry);
                        writer.Write(entryBytes);
                    }

                    // Save the modified flash data based on the file version (32-bit or 64-bit)
                    if (iggy.is_64)
                    {
                        iggy.SaveFlashData64(writer);
                    }
                    else
                    {
                        iggy.SaveFlashData32(writer);
                    }
                }
                Console.WriteLine($"Modified IGGY file saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving modified IGGY file: {ex.Message}");
            }
        }

        private void RedirectConsoleOutput()
        {
            try
            {
                var writer = new RichTextBoxWriter(consoleLog);
                Console.SetOut(writer);
                Console.WriteLine("Console output redirected successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error redirecting console output: {ex.Message}");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Iggy Files|*.iggy";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Save AS3 Names to a separate file
                    SaveAS3Names(Path.ChangeExtension(saveFileDialog.FileName, "as3_names.bin"));

                    // Save AS3 Code to a separate file
                    SaveAS3Code(Path.ChangeExtension(saveFileDialog.FileName, "as3_code.bin"));

                    // Save other data to separate files
                    SaveMainSection(Path.ChangeExtension(saveFileDialog.FileName, "main_section.bin"));
                    SaveNamesSection(Path.ChangeExtension(saveFileDialog.FileName, "names_section.bin"));
                    SaveLastSection(Path.ChangeExtension(saveFileDialog.FileName, "last_section.bin"));

                    MessageBox.Show("Files saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SaveAS3Names(string filePath)
        {
            if (iggy.as3_names_section != null && iggy.as3_names_section.Count != 0)
            {
                File.WriteAllBytes(filePath, iggy.as3_names_section.ToArray());
                Console.WriteLine($"AS3 Names saved to: {filePath}");
            }
            else
            {
                Console.WriteLine("AS3 Names section is empty.");
            }
        }

        public void SaveAS3Code(string filePath)
        {
            if (iggy.as3_code_section != null && iggy.as3_code_section.Count != 0)
            {
                File.WriteAllBytes(filePath, iggy.as3_code_section.ToArray());
                Console.WriteLine($"AS3 Code saved to: {filePath}");
            }
            else
            {
                Console.WriteLine("AS3 Code section is empty.");
            }
        }

        public void SaveMainSection(string filePath)
        {
            List<byte> otherData = new List<byte>();

            // Add other data sections if needed
            otherData.AddRange(iggy.main_section);


            if (otherData.Count != 0)
            {
                File.WriteAllBytes(filePath, otherData.ToArray());
                Console.WriteLine($"Other data saved to: {filePath}");
            }
            else
            {
                Console.WriteLine("Other data sections are empty.");
            }
        }
        public void SaveNamesSection(string filePath)
        {
            List<byte> otherData = new List<byte>();

            // Add other data sections if needed
            otherData.AddRange(iggy.names_section);


            if (otherData.Count != 0)
            {
                File.WriteAllBytes(filePath, otherData.ToArray());
                Console.WriteLine($"Other data saved to: {filePath}");
            }
            else
            {
                Console.WriteLine("Other data sections are empty.");
            }
        }
        public void SaveLastSection(string filePath)
        {
            List<byte> otherData = new List<byte>();

            // Add other data sections if needed
            otherData.AddRange(iggy.last_section);


            if (otherData.Count != 0)
            {
                File.WriteAllBytes(filePath, otherData.ToArray());
                Console.WriteLine($"Other data saved to: {filePath}");
            }
            else
            {
                Console.WriteLine("Other data sections are empty.");
            }
        }
        private void extractSubFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractSubfiles(iggyPath);
        }
        public void ExtractSubfiles(string iggyFilePath)
        {
            byte[] iggyFile = File.ReadAllBytes(iggyFilePath);

            for (int i = 0; i < iggy.header.num_subfiles; i++)
            {
                string subfilePath = $"subfile_{i + 1}.bin"; // Choose an appropriate file name

                // Add debugging statements
                Console.WriteLine($"Extracting subfile: {subfilePath}");
                Console.WriteLine($"Offset: {iggy.subFileEntries[i].offset}, Size: {iggy.subFileEntries[i].size}, File Length: {iggyFile.Length}");

                ExtractSubfile(iggyFile, iggy.subFileEntries[i], subfilePath);
            }

        }
        private void ExtractSubfile(byte[] iggyFile, Iggy.IGGYSubFileEntry subfileEntry, string outputFilePath)
        {
            try
            {
                // Output detailed debug information
                Console.WriteLine($"Extracting subfile: {outputFilePath}");
                Console.WriteLine($"Offset: {subfileEntry.offset}, Size: {subfileEntry.size}, File Length: {iggyFile.Length}");
                Console.WriteLine($"Attempting to extract subfile: {outputFilePath}");
                Console.WriteLine($"Length of iggyFile array: {iggyFile.Length}");
                Console.WriteLine($"Calculated Offset: {subfileEntry.offset}, Calculated Size: {subfileEntry.size}");

                Console.WriteLine($"Subfile Entry ID: {subfileEntry.id}");
                Console.WriteLine($"Subfile Entry Size: {subfileEntry.size}");
                Console.WriteLine($"Subfile Entry Offset: {subfileEntry.offset}");
                Console.WriteLine($"iggyFile.Length: {iggyFile.Length}");


                // Create a FileStream to read the iggyFile
                using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                {
                    // Create a BinaryWriter to write to the FileStream
                    using (BinaryWriter writer = new BinaryWriter(fileStream))
                    {
                        // Create a BinaryReader to read from the iggyFile
                        using (BinaryReader reader = new BinaryReader(new MemoryStream(iggyFile)))
                        {
                            // Set the position of the reader to the subfile's offset
                            reader.BaseStream.Position = subfileEntry.offset;

                            // Read the subfile data from the iggyFile and write it to the FileStream
                            byte[] subfileData = reader.ReadBytes((int)subfileEntry.size);
                            writer.Write(subfileData);
                        }
                    }
                }

                Console.WriteLine($"Subfile extracted: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting subfile: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }


        public class RichTextBoxWriter : TextWriter
        {
            private readonly RichTextBox _richTextBox;

            public RichTextBoxWriter(RichTextBox richTextBox)
            {
                _richTextBox = richTextBox;
            }

            public override void Write(char value)
            {
                // Append the character to the RichTextBox
                _richTextBox.AppendText(value.ToString());
            }

            public override Encoding Encoding => Encoding.Default;
        }
    }
}

