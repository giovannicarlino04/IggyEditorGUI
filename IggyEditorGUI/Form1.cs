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
                        richTextBox1.Text = $"Signature: 0x{iggy.header.signature:X}\n";
                        richTextBox1.Text += $"Version: {iggy.header.version}\n";
                        richTextBox1.Text += $"Platform: {BitConverter.ToString(iggy.header.platform)}\n";
                        richTextBox1.Text += $"Unknown Field (unk_0C): {iggy.unk_0C}\n";
                        richTextBox1.Text += $"Number of Subfiles: {iggy.header.num_subfiles}\n";
                        richTextBox1.Text += $"Subfiles:\n";

                        for (int i = 0; i < iggy.header.num_subfiles; i++)
                        {
                            richTextBox1.Text += $"Subfile {i + 1}:\n";
                            richTextBox1.Text += $"ID: {iggy.subFileEntries[i].id}\n";
                            richTextBox1.Text += $"Size: {iggy.subFileEntries[i].size}\n";
                            richTextBox1.Text += $"Size2: {iggy.subFileEntries[i].size2}\n";
                            richTextBox1.Text += $"Offset: {iggy.subFileEntries[i].offset}\n";
                            richTextBox1.Text += "\n";
                        }

                        // You can add more details about other fields if needed

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
        private void RedirectConsoleOutput()
        {
            var writer = new RichTextBoxWriter(richTextBox2);
            Console.SetOut(writer);
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

                    // Save other data to a separate file
                    SaveOtherData(Path.ChangeExtension(saveFileDialog.FileName, "other_data.bin"));

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

        public void SaveOtherData(string filePath)
        {
            List<byte> otherData = new List<byte>();

            // Add other data sections if needed
            otherData.AddRange(iggy.main_section);
            otherData.AddRange(iggy.names_section);
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
            Iggy iggy = new Iggy();

            if (iggy.Load(iggyFile))
            {
                for (int i = 0; i < iggy.header.num_subfiles; i++)
                {
                    string subfilePath = $"subfile_{i + 1}.bin"; // Choose an appropriate file name
                    ExtractSubfile(iggyFile, iggy.subFileEntries[i], subfilePath);
                }
            }
        }

        private void ExtractSubfile(byte[] iggyFile, Iggy.IGGYSubFileEntry subfileEntry, string outputFilePath)
        {
            try
            {
                // Extract subfile data from iggyFile based on subfileEntry information
                byte[] subfileData = new byte[subfileEntry.size];
                Array.Copy(iggyFile, subfileEntry.offset, subfileData, 0, subfileEntry.size);

                // Save the extracted subfile to a new file
                File.WriteAllBytes(outputFilePath, subfileData);
                Console.WriteLine($"Subfile extracted: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting subfile: {ex.Message}");
            }
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
