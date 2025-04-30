# 📦 ZipHub - Zip File Utility

A lightweight desktop tool built with C# (.NET) for zipping files and folders. Ideal for users who need a simple way to compress content into `.zip` archives.

---

## 🔍 Features

- ✅ Compress individual files or entire directories into a `.zip` archive
- 📁 Batch processing support — select multiple files/folders at once
- 🖥️ Clean and intuitive Windows Forms GUI
- 📤 Easily extract zipped content using built-in Windows tools

---

## 🧰 Tech Stack

- C# (.NET Framework)
- Windows Forms (WinForms)
- `System.IO.Compression` (for compression logic)

---

## ⚙️ Requirements

- .NET Framework 4.7.2 or higher
- Visual Studio 2019 or later

---

## 🗂️ Project Structure

- `ZipHub`: Main WinForms application
- `ZipHelper.cs`: Handles zip logic using .NET’s compression libraries
- `MainForm.cs`: UI logic and user interactions

---

## 🚀 How to Run

```bash
# 1. Clone the repository
git clone https://github.com/codebanoo/ziphub.git

# 2. Open the solution in Visual Studio
ZipHub.sln

# 3. Build the project (choose Release or Debug)

# 4. Navigate to the output folder
cd ZipHub/bin/Release

# 5. Run the executable
ZipHub.exe
