﻿using MenuTools.Methods;
using MenuTools.Properties;
using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[assembly: CLSCompliant(false)]
namespace MenuTools
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".lnk")]
    [DisplayName("MenuTools")]
    public class MenuToolsFiles : SharpContextMenu
    {
        private static readonly RegistryKey MenuToolsSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MenuTools\\Settings", true);
        private ContextMenuStrip Menu;
        private ToolStripMenuItem MenuToolsMenu, OpenNotepad, BlockFirewall, CopyPath, CopyName, Attributes, SymLink, TakeOwnership;
        private ToolStripMenuItem AttributesMenu, HiddenAttributes, SystemAttributes, ReadOnlyAttributes;
        protected override bool CanShowMenu()
        {
            return true;
        }
        protected override ContextMenuStrip CreateMenu()
        {
            CheckUserSettings();
            // Main Menu
            using (Menu = new ContextMenuStrip())
            {
                Menu.Name = "MenuToolsFiles";

                using (MenuToolsMenu = new ToolStripMenuItem())
                {
                    MenuToolsMenu.Name = "MenuToolsMenu";

                    // OpenNotepad
                    using (OpenNotepad = new ToolStripMenuItem())
                    {
                        OpenNotepad.Text = Resources.OpenNotepad;
                        OpenNotepad.Name = "OpenNotepad";
                    }
                    // BlockFirewall
                    using (BlockFirewall = new ToolStripMenuItem())
                    {
                        BlockFirewall.Text = Resources.BlockText;
                        BlockFirewall.Name = "BlockFirewall";
                    }
                    // CopyPath
                    using (CopyPath = new ToolStripMenuItem())
                    {
                        CopyPath.Text = Resources.CopyPathText;
                        CopyPath.Name = "CopyPath";
                    }
                    // CopyName
                    using (CopyName = new ToolStripMenuItem())
                    {
                        CopyName.Text = Resources.CopyNameText;
                        CopyName.Name = "CopyName";
                    }
                    // Attributes
                    using (Attributes = new ToolStripMenuItem())
                    {
                        Attributes.Name = "Attributes";

                        Attributes.Text = Resources.AttributesText;
                        // AttributesMenu
                        using (AttributesMenu = new ToolStripMenuItem())
                        {
                            AttributesMenu.Text = Resources.AttributesMenu;
                            AttributesMenu.Name = "AttributesMenu";
                        }
                        // Get : Set Attributes
                        string[] SelectedPath = SelectedItemPaths.Cast<string>().ToArray();
                        if (SelectedPath.Length > 1)
                        {
                            foreach (string item in SelectedPath)
                            {
                                AttributesInfo.GetFileAttributes(item);
                            }
                        }
                        else
                        {
                            AttributesInfo.GetFileAttributes(SelectedPath.ToStringArray(false));
                        }
                        SetFileAttributes();
                    }
                    // SymLink
                    using (SymLink = new ToolStripMenuItem())
                    {
                        SymLink.Text = Resources.CreateSymbolicLink;
                        SymLink.Name = "SymLink";
                    }
                    // TakeOwnership
                    using (TakeOwnership = new ToolStripMenuItem())
                    {
                        TakeOwnership.Text = Resources.TakeOwnershipText;
                        TakeOwnership.Name = "TakeOwnership";
                    }
                }
            }
            MenuDeveloper();

            return Menu;
        }
        private static void CheckUserSettings()
        {
            if (MenuToolsSettings == null)
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                Software.CreateSubKey("MenuTools");
                RegistryKey SoftwareMenuTools = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MenuTools", true);
                SoftwareMenuTools.CreateSubKey("Settings");
                // All Files
                MenuToolsSettings.SetValue("OpenNotepadFiles", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("BlockWithFirewallFiles", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("CopyPathFiles", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("CopyNameFiles", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("AttributesFiles", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("SymlinkFiles", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("TakeOwnershipFiles", 0x00000001, RegistryValueKind.DWord);
                // All Files Shorcuts
                MenuToolsSettings.SetValue("AttributesShortcuts", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("OpenNotepadShort", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("CopyPathShortFiles", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("CopyNameShortFiles", 0x00000001, RegistryValueKind.DWord);
                // Directories
                MenuToolsSettings.SetValue("BlockWithFirewallDirectory", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("CopyPathDirectory", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("CopyNameDirectory", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("AttributesDirectory", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("SymlinkDirectory", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("TakeOwnershipDirectory", 0x00000001, RegistryValueKind.DWord);
                // Directory Background
                MenuToolsSettings.SetValue("AttributesDirectoryBack", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("CommandLinesDirectoryBack", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("FindWallpaperDirectoryBack", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("SystemFoldersDirectoryBack", 0x00000001, RegistryValueKind.DWord);
                MenuToolsSettings.SetValue("PasteContentsDirectoryBack", 0x00000001, RegistryValueKind.DWord);
            }
        }
        private void MenuDeveloper()
        {
            // Main Menu
            Menu.Items.Add(MenuToolsMenu);
            MenuToolsMenu.Text = Resources.MenuToolsName;

            // Icons
            MenuToolsMenu.Image = Resources.MAIN_ICON.ToBitmap();
            OpenNotepad.Image = Resources.notepad.ToBitmap();
            BlockFirewall.Image = Resources.Firewall.ToBitmap();
            CopyPath.Image = Resources.CopyPath.ToBitmap();
            CopyName.Image = Resources.CopyName.ToBitmap();
            Attributes.Image = Resources.FileAttributes.ToBitmap();
            AttributesMenu.Image = Resources.MAIN_ICON.ToBitmap();

            SymLink.Image = Resources.SymLink.ToBitmap();
            TakeOwnership.Image = Resources.TakeOwnership.ToBitmap();

            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            AddMenuItems(array);

            // Subscriptions
            OpenNotepad.Click += (sender, args) => OpenNotepadMethod();
            BlockFirewall.Click += (sender, args) => BlockFirewallMethod();
            CopyPath.Click += (sender, args) => CopyPathMethod();
            CopyName.Click += (sender, args) => CopyNameMethod();
            AttributesMenu.Click += (sender, args) => AttributesMenuMethod();

            HiddenAttributes.Click += (sender, args) => HiddenAttributesMethod();
            SystemAttributes.Click += (sender, args) => SystemAttributesMethod();
            ReadOnlyAttributes.Click += (sender, args) => ReadOnlyAttributesMethod();
            SymLink.Click += (sender, args) => SymLinkMethod();
            TakeOwnership.Click += (sender, args) => TakeOwnershipMethod();
        }

        // Add Menu Items
        private void AddMenuItems(string[] array)
        {
            try
            {
                // Disabler
                bool isShortcut = false;
                bool isExeDllFile = false;
                foreach (string path in array)
                {
                    try
                    {
                        if (Path.GetExtension(path) == ".lnk")
                        {
                            isShortcut = true;
                        }
                        if (Path.GetExtension(path) != ".exe" && Path.GetExtension(path) != ".dll")
                        {
                            isExeDllFile = false;
                        }
                        if (Path.GetExtension(path) == ".exe" || Path.GetExtension(path) == ".dll")
                        {
                            isExeDllFile = true;
                        }
                        if (Directory.Exists(ShortcutHandler.GetShortcutTarget(path)))
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " Error at ShortcutHandler.GetShortcutTarget(path)");
                        continue;
                    }
                }
                object OpenNotepadFiles = MenuToolsSettings.GetValue("OpenNotepadFiles");
                if (OpenNotepadFiles != null)
                {
                    if (OpenNotepadFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(OpenNotepad);
                    }
                }
                object BlockWithFirewallFiles = MenuToolsSettings.GetValue("BlockWithFirewallFiles");
                if (BlockWithFirewallFiles != null)
                {
                    if (BlockWithFirewallFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(BlockFirewall);
                    }
                }
                object CopyPathFiles = MenuToolsSettings.GetValue("CopyPathFiles");
                if (CopyPathFiles != null && !isShortcut)
                {
                    if (CopyPathFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(CopyPath);
                    }
                }
                object CopyNameFiles = MenuToolsSettings.GetValue("CopyNameFiles");
                if (CopyNameFiles != null && !isShortcut)
                {
                    if (CopyNameFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(CopyName);
                    }
                }
                object AttributesFiles = MenuToolsSettings.GetValue("AttributesFiles");
                if (AttributesFiles != null)
                {
                    if (AttributesFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(Attributes);
                        Attributes.DropDownItems.Add(AttributesMenu);
                        Attributes.DropDownItems.Add(new ToolStripSeparator());
                        Attributes.DropDownItems.Add(HiddenAttributes);
                        Attributes.DropDownItems.Add(SystemAttributes);
                        Attributes.DropDownItems.Add(ReadOnlyAttributes);
                    }
                }
                object SymlinkFiles = MenuToolsSettings.GetValue("SymlinkFiles");
                if (SymlinkFiles != null)
                {
                    if (SymlinkFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(SymLink);
                    }
                }
                object TakeOwnershipFiles = MenuToolsSettings.GetValue("TakeOwnershipFiles");
                if (TakeOwnershipFiles != null)
                {
                    if (TakeOwnershipFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(TakeOwnership);
                    }
                }
                object AttributesShortcuts = MenuToolsSettings.GetValue("AttributesShortcuts");
                if (AttributesShortcuts != null && isShortcut)
                {
                    if (AttributesShortcuts.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(Attributes);
                    }
                    else
                    {
                        Attributes.Dispose();
                    }
                }
                object OpenNotepadShort = MenuToolsSettings.GetValue("OpenNotepadShort");
                if (OpenNotepadShort != null && isShortcut)
                {
                    if (OpenNotepadShort.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(OpenNotepad);
                    }
                    else
                    {
                        OpenNotepad.Dispose();
                    }
                }
                object CopyPathShortFiles = MenuToolsSettings.GetValue("CopyPathShortFiles");
                if (CopyPathShortFiles != null && isShortcut)
                {
                    if (CopyPathShortFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(CopyPath);
                    }
                }
                object CopyNameShortFiles = MenuToolsSettings.GetValue("CopyNameShortFiles");
                if (CopyNameShortFiles != null && isShortcut)
                {
                    if (CopyNameShortFiles.ToString() == "1")
                    {
                        MenuToolsMenu.DropDownItems.Add(CopyName);
                    }
                }

                MenuItemDisabler(isShortcut, isExeDllFile);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MenuItemDisabler(bool isShortcut, bool isExeDllFile)
        {
            if (isShortcut)
            {
                if (BlockFirewall != null)
                {
                    BlockFirewall.Dispose();
                }

                if (SymLink != null)
                {
                    SymLink.Dispose();
                }

                if (TakeOwnership != null)
                {
                    TakeOwnership.Dispose();
                }
            }
            if (isExeDllFile)
            {
                if (OpenNotepad != null)
                {
                    OpenNotepad.Dispose();
                }
            }
            if (!isExeDllFile)
            {
                if (BlockFirewall != null)
                {
                    BlockFirewall.Dispose();
                }
            }
            bool AllDisabled = true;
            foreach (ToolStripMenuItem item in MenuToolsMenu.DropDownItems)
            {
                if (item != null)
                {
                    AllDisabled = false;
                }
            }
            if (AllDisabled)
            {
                Menu.Dispose();
            }
        }
        // Set File Attributes
        private void SetFileAttributes()
        {
            using (HiddenAttributes = new ToolStripMenuItem())
            {
                HiddenAttributes.Text = Resources.HiddenAttributes;
                HiddenAttributes.Name = "HiddenAttributes";
            }
            if (AttributesInfo.hidden)
            {
                HiddenAttributes.Image = Resources.AttributesShow.ToBitmap();
            }
            // SystemAttributes
            using (SystemAttributes = new ToolStripMenuItem())
            {
                SystemAttributes.Text = Resources.SystemAttributes;
                SystemAttributes.Name = "SystemAttributes";
            }
            if (AttributesInfo.system)
            {
                SystemAttributes.Image = Resources.AttributesShow.ToBitmap();
            }
            // ReadOnlyAttributes
            using (ReadOnlyAttributes = new ToolStripMenuItem())
            {
                ReadOnlyAttributes.Text = Resources.ReadOnlyAttributes;
                ReadOnlyAttributes.Name = "ReadOnlyAttributes";
            }
            if (AttributesInfo.readOnly)
            {
                ReadOnlyAttributes.Image = Resources.AttributesShow.ToBitmap();
            }
        }
        // Methods
        private void OpenNotepadMethod()
        {
            foreach (string filePath in SelectedItemPaths)
            {
                StartProcess.StartInfo("Notepad.exe", filePath);
            }
        }
        private void BlockFirewallMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\MenuTools.exe", "\"" + array.ToStringArray() + "\" " + "-firewallfiles", false, true);
        }
        private void CopyPathMethod()
        {
            Clipboard.Clear();
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            Clipboard.SetText(array.ToStringArray(false));
        }
        private void CopyNameMethod()
        {
            Clipboard.Clear();
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            Clipboard.SetText(array.ToStringArray(true));
        }
        private void AttributesMenuMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\MenuTools.exe", "\"" + array.ToStringArray(false) + "\" " + "-attributesmenu");
        }
        private void HiddenAttributesMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            foreach (string item in array)
            {
                FileAttributes attributes = File.GetAttributes(item);
                if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.Hidden);
                    File.SetAttributes(item, attributes);
                }
                else
                {
                    File.SetAttributes(item, File.GetAttributes(item) | FileAttributes.Hidden);
                }
            }
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\MenuTools.exe", "-refresh");
        }
        private void SystemAttributesMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            foreach (string item in array)
            {
                FileAttributes attributes = File.GetAttributes(item);
                if ((attributes & FileAttributes.System) == FileAttributes.System)
                {
                    attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.System);
                    File.SetAttributes(item, attributes);
                }
                else
                {
                    File.SetAttributes(item, File.GetAttributes(item) | FileAttributes.System);
                }
            }
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\MenuTools.exe", "-refresh");
        }
        private void ReadOnlyAttributesMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            foreach (string item in array)
            {
                FileAttributes attributes = File.GetAttributes(item);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.ReadOnly);
                    File.SetAttributes(item, attributes);
                }
                else
                {
                    File.SetAttributes(item, File.GetAttributes(item) | FileAttributes.ReadOnly);
                }
            }
        }
        private void SymLinkMethod()
        {
            foreach (string file in SelectedItemPaths)
            {
                try
                {
                    using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                    {
                        ofd.Description = Path.GetFileName(file);
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            string PathName = ofd.SelectedPath + @"\" + Path.GetFileName(file);
                            StartProcess.StartInfo("cmd.exe", "/c mklink " + "\"" + PathName + "\"" + " " + "\"" + file + "\"", true, true, true);
                        }
                    }
                }
                catch (System.ComponentModel.Win32Exception ex) { StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\MenuTools.exe", "\"" + ex.Message + "\"" + " -catchhandler", false, false, false); }
            }
        }
        private void TakeOwnershipMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\MenuTools.exe", "\"" + array.ToStringArray(false) + "\"" + " -ownership", false, true);
        }
    }
}
