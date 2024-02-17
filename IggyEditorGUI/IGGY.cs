using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace IggyEditorGUI
{
    public class Iggy
    {
        private const uint IGGY_SIGNATURE = 0xED0A6749;

        public IGGYHeader header;
        public IGGYSubFileEntry subFileEntry;
        public IGGYSubFileEntry[] subFileEntries;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IGGYHeader
        {
            public uint signature;
            public uint version;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] platform;
            public uint unk_0C;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xC)]
            public byte[] reserved;
            public uint num_subfiles;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IGGYSubFileEntry
        {
            public uint id;
            public uint size;
            public uint size2;
            public uint offset;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IGGYFlashHeader32
        {
            public uint main_offset;        // 0
            public uint as3_section_offset; // 4
            public uint unk_offset;         // 8
            public uint unk_offset2;        // 0xC
            public uint unk_offset3;        // 0x10
            public uint unk_offset4;        // 0x14
            public ulong unk_18;            // 0x18
            public uint width;              // 0x20
            public uint height;             // 0x24
            public uint unk_28;             // 0x28
            public uint unk_2C;
            public uint unk_30;
            public uint unk_34;
            public uint unk_38;
            public uint unk_3C;
            public float unk_40;
            public uint unk_44;
            public uint unk_48;
            public uint unk_4C;
            public uint names_offset;       // 0x50
            public uint unk_offset5;        // 0x54
            public ulong unk_58;            // 0x58
            public uint last_section_offset; // 0x60
            public uint unk_offset6;        // 0x64
            public uint as3_code_offset;    // 0x68
            public uint as3_names_offset;   // 0x6C
            public uint unk_70;
            public uint unk_74;
            public uint unk_78;
            public uint unk_7C;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IGGYFlashHeader64
        {
            public ulong main_offset;       // 0
            public ulong as3_section_offset; // 8
            public ulong unk_offset;         // 0x10
            public ulong unk_offset2;        // 0x18
            public ulong unk_offset3;        // 0x20
            public ulong unk_offset4;        // 0x28
            public uint unk_30;             // 0x30
            public uint width;               // 0x34
            public uint height;              // 0x38
            public uint unk_40;              // 0x3C
            public uint unk_44;              // 0x40
            public uint unk_48;              // 0x44
            public uint unk_4C;              // 0x48
            public uint unk_50;              // 0x4C
            public uint unk_54;              // 0x50
            public float unk_58;             // 0x54
            public uint unk_5C;              // 0x58
            public ulong unk_60;             // 0x60
            public ulong unk_68;             // 0x68
            public ulong names_offset;       // 0x70
            public ulong unk_offset5;        // 0x78
            public ulong unk_80;             // 0x80
            public ulong last_section_offset; // 0x88
            public ulong unk_offset6;        // 0x90
            public ulong as3_code_offset;    // 0x98
            public ulong as3_names_offset;   // 0xA0
            public uint unk_A8;              // 0xA8
            public uint unk_AC;              // 0xAC
            public uint unk_B0;              // 0xB0
            public uint unk_B4;              // 0xB4
        }

        public bool is_64;
        public uint unk_0C;
        public List<byte> index_data;
        public uint as3_offset;
        public List<byte> main_section;
        public List<byte> as3_names_section;
        public List<byte> as3_code_section;
        public List<byte> names_section;
        public List<byte> last_section;

        public Iggy()
        {
            header = new IGGYHeader();
            subFileEntries = new IGGYSubFileEntry[header.num_subfiles]; // Initialize the array with the correct size

            index_data = new List<byte>();
            main_section = new List<byte>();
            as3_names_section = new List<byte>();
            as3_code_section = new List<byte>();
            names_section = new List<byte>();
            last_section = new List<byte>();
        }

        private void LoadMainSection(byte[] buf, uint offset, uint size)
        {
            main_section = new List<byte>(buf.Skip((int)offset).Take((int)size));
        }

        private void LoadAS3NamesSection(byte[] buf, uint offset, uint size)
        {
            as3_names_section = new List<byte>(buf.Skip((int)offset).Take((int)size));
        }

        private void LoadNamesSection(byte[] buf, uint offset, uint size)
        {
            names_section = new List<byte>(buf.Skip((int)offset).Take((int)size));
        }

        private void LoadLastSection(byte[] buf, uint offset, uint size)
        {
            last_section = new List<byte>(buf.Skip((int)offset).Take((int)size));
        }
        private void LoadFlashData32(byte[] buf)
        {
            IGGYFlashHeader32 flashHeader32 = ByteArrayToStructure<IGGYFlashHeader32>(buf);

            // Calculate the size of the AS3 Code Section
            uint calculatedSize = 0;

            calculatedSize = (uint)Math.Min(flashHeader32.names_offset - flashHeader32.as3_code_offset, buf.Length - (int)flashHeader32.as3_code_offset);

            // Adjust the condition to use calculatedSize instead of buf.Length - flashHeader32.as3_code_offset
            if (calculatedSize > 0 && calculatedSize <= calculatedSize)
                // Load Main Section
                LoadMainSection(buf, flashHeader32.main_offset, flashHeader32.as3_section_offset - flashHeader32.main_offset);

                // Load AS3 Names Section
                LoadAS3NamesSection(buf, flashHeader32.as3_names_offset, flashHeader32.last_section_offset - flashHeader32.as3_names_offset);

                // Load AS3 Code Section
                as3_code_section = new List<byte>(buf.Skip((int)flashHeader32.as3_code_offset).Take((int)calculatedSize));

                // Print AS3 Code Section details for debugging
                // Console.WriteLine($"AS3 Code Section: {BitConverter.ToString(as3_code_section.ToArray())}");

                // Load Names Section
                LoadNamesSection(buf, flashHeader32.names_offset, flashHeader32.last_section_offset - flashHeader32.names_offset);

                // Load Last Section
                LoadLastSection(buf, flashHeader32.last_section_offset, flashHeader32.main_offset + flashHeader32.width * flashHeader32.height);

        }
        private void LoadFlashData64(byte[] buf)
        {
            IGGYFlashHeader64 flashHeader64 = ByteArrayToStructure<IGGYFlashHeader64>(buf);

            // Calculate the size of the AS3 Code Section
            uint calculatedSize = (uint)Math.Min((decimal)(flashHeader64.names_offset - flashHeader64.as3_code_offset), (decimal)(buf.Length - (int)flashHeader64.as3_code_offset));

            // Adjust the condition to use calculatedSize instead of buf.Length - flashHeader64.as3_code_offset
            if (calculatedSize > 0 && calculatedSize <= calculatedSize)
            {
                // Load Main Section
                LoadMainSection(buf, (uint)flashHeader64.main_offset, (uint)(flashHeader64.as3_section_offset - flashHeader64.main_offset));

                // Load AS3 Names Section
                LoadAS3NamesSection(buf, (uint)flashHeader64.as3_names_offset, (uint)(flashHeader64.last_section_offset - flashHeader64.as3_names_offset));

                // Load AS3 Code Section
                as3_code_section = new List<byte>(buf.Skip((int)flashHeader64.as3_code_offset).Take((int)calculatedSize));

                // Print AS3 Code Section details for debugging
                // Console.WriteLine($"AS3 Code Section: {BitConverter.ToString(as3_code_section.ToArray())}");

                // Load Names Section
                LoadNamesSection(buf, (uint)flashHeader64.names_offset, (uint)(flashHeader64.last_section_offset - flashHeader64.names_offset));

                // Load Last Section
                LoadLastSection(buf, (uint)flashHeader64.last_section_offset, (uint)(flashHeader64.main_offset + flashHeader64.width * flashHeader64.height));
            }
        }
        public bool Load(byte[] buf)
        {
            try
            {
                header = ByteArrayToStructure<IGGYHeader>(buf);

                if (header.signature != IGGY_SIGNATURE)
                {
                    Console.WriteLine("Invalid IGGY file. Signature mismatch.");
                    return false;
                }

                Console.WriteLine($"Signature: 0x{header.signature:X}");
                Console.WriteLine($"Version: {header.version}");
                Console.WriteLine($"Platform: {BitConverter.ToString(header.platform)}");
                Console.WriteLine($"Unknown Field (unk_0C): {header.unk_0C}");
                Console.WriteLine($"Number of Subfiles: {header.num_subfiles}");

                // Verify array initialization
                subFileEntries = new IGGYSubFileEntry[header.num_subfiles];

                // Verify loop boundaries
                int subFileEntryOffset = Marshal.SizeOf(typeof(IGGYHeader));
                Console.WriteLine($"Number of Subfiles: {header.num_subfiles}");
                Console.WriteLine($"subFileEntryOffset: {subFileEntryOffset}");
                Console.WriteLine($"Remaining bytes: {buf.Length - subFileEntryOffset}");

                for (int i = 0; i < header.num_subfiles; i++)
                {
                    Console.WriteLine($"Iteration {i + 1}:");

                    // Ensure sufficient remaining bytes for subfile entry
                    if (buf.Length - subFileEntryOffset < Marshal.SizeOf(typeof(IGGYSubFileEntry)))
                    {
                        Console.WriteLine("Error: Insufficient remaining bytes for subfile entry.");
                        return false;
                    }

                    // Parse subfile entry
                    subFileEntries[i].id = BitConverter.ToUInt32(buf, subFileEntryOffset);
                    subFileEntries[i].size = BitConverter.ToUInt32(buf, subFileEntryOffset + sizeof(uint));
                    subFileEntries[i].size2 = BitConverter.ToUInt32(buf, subFileEntryOffset + 2 * sizeof(uint));
                    subFileEntries[i].offset = BitConverter.ToUInt32(buf, subFileEntryOffset + 3 * sizeof(uint));

                    Console.WriteLine($"ID: {subFileEntries[i].id}");
                    Console.WriteLine($"Size: {subFileEntries[i].size}");
                    Console.WriteLine($"Size2: {subFileEntries[i].size2}");
                    Console.WriteLine($"Offset: {subFileEntries[i].offset}");

                    subFileEntryOffset += Marshal.SizeOf(typeof(IGGYSubFileEntry));
                }

                // Only continue loading if the header is valid
                if (is_64)
                {
                    LoadFlashData64(buf);
                }
                else
                {
                    LoadFlashData32(buf);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading IGGY file: {ex.Message}");
                return false;
            }
        }


        public void SaveFlashData32(BinaryWriter writer)
        {
            // Save Main Section
            subFileEntry.size = (uint)main_section.Count;
            writer.Write(main_section.ToArray());

            // Save AS3 Names Section
            subFileEntry.size2 = (uint)as3_names_section.Count;
            writer.Write(as3_names_section.ToArray());

            // Save AS3 Code Section
            if (as3_code_section != null && as3_code_section.Count != 0)
            {
                subFileEntry.offset = (uint)(writer.BaseStream.Position + sizeof(uint));
                subFileEntry.size = (uint)as3_code_section.Count;
                writer.Write(as3_code_section.ToArray());
            }

            // Save Names Section
            subFileEntry.size2 = (uint)names_section.Count;
            writer.Write(names_section.ToArray());

            // Save Last Section
            subFileEntry.size = (uint)last_section.Count;
            writer.Write(last_section.ToArray());
        }

        public void SaveFlashData64(BinaryWriter writer)
        {
            // Save Main Section
            subFileEntry.size = (uint)main_section.Count;
            writer.Write(main_section.ToArray());

            // Save AS3 Names Section
            subFileEntry.size2 = (uint)as3_names_section.Count;
            writer.Write(as3_names_section.ToArray());

            // Save AS3 Code Section
            if (as3_code_section != null && as3_code_section.Count != 0)
            {
                subFileEntry.offset = (uint)(writer.BaseStream.Position + sizeof(ulong));
                subFileEntry.size = (uint)as3_code_section.Count;
                writer.Write(as3_code_section.ToArray());
            }

            // Save Names Section
            subFileEntry.size2 = (uint)names_section.Count;
            writer.Write(names_section.ToArray());

            // Save Last Section
            subFileEntry.size = (uint)last_section.Count;
            writer.Write(last_section.ToArray());
        }

        private int GetFlashDataSize()
        {
            int size = main_section.Count + as3_names_section.Count + as3_code_section.Count + names_section.Count + last_section.Count;
            return size;
        }

        public void PrintIndex()
        {
            return;
        }
        private byte[] StructureToByteArray<T>(T structure) where T : struct
        {
            int size = Marshal.SizeOf(structure);
            byte[] byteArray = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, ptr, false);
                Marshal.Copy(ptr, byteArray, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return byteArray;
        }
        public T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }
    }
}