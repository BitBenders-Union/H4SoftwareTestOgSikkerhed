# Step-by-Step Guide: Installing Visual Studio, WSL 1, and Running .NET 8 Projects with SQLite

This guide will walk you through the process of:

1. Installing Visual Studio on your Windows machine,
2. Installing and configuring WSL 1 with Ubuntu,
3. Manually enabling the required Windows features for WSL,
4. Fixing the Untrusted Certificate error in ASP.NET Core,
5. Replacing LocalDB with SQLite in your .NET 8 project,
6. Running your .NET 8 project using WSL in Visual Studio,
7. Applying EF Core migrations to create your MockDB,
8. Setting up your own certificate for HTTPS,
9. Setting up Docker to run your project.

---

## Table of Contents

1. [Install Visual Studio](#install-visual-studio)
2. [Install WSL](#install-wsl)
3. [Manually Enable WSL Features](#manually-enable-wsl-features)
4. [Change DNS to 8.8.8.8](#change-dns-to-8888)
5. [Fix Untrusted Certificate Error](#fix-untrusted-certificate-error)
6. [Use SQLite for Database](#use-sqlite-for-database)
7. [Run .NET 8 Project in Visual Studio as WSL](#run-net-8-project-in-visual-studio-as-wsl)
8. [Apply EF Core Migrations to Create MockDB](#apply-ef-core-migrations-to-create-mockdb)
9. [Setup Your Own Certificate](#setup-your-own-certificate)
10. [Run Your Project in Docker](#run-your-project-in-docker)

---

## 1. Install Visual Studio

Follow these steps to install Visual Studio on your Windows machine:

1. **Go to the Visual Studio website:**

   - Visit the [Visual Studio download page](https://visualstudio.microsoft.com/downloads/).

2. **Download the installer:**

   - Choose the **Community edition** (free for individual use) or another edition that suits your needs.
   - Download the installer and run it.

3. **Select the necessary workload:**

   - When the installer opens, select the **Desktop development with C++** workload. This workload includes the necessary compilers and libraries for C++ or C# development.
   - You can choose additional workloads based on your needs, but **Visual Studio Code** does not need to be selected.

4. **Complete the installation:**

   - Click **Install** to start the process. Visual Studio will download and install the selected components.
   - Once installed, launch Visual Studio to ensure it’s working properly.

---

## 2. Install WSL

After installing Visual Studio, proceed with installing WSL 1 and configuring it with Ubuntu.

1. **Open PowerShell as Administrator:**

   - Right-click on the Start button, and select **Windows Terminal (Admin)** or **PowerShell (Admin)**.

2. **Install WSL:**

   - Run the following command in PowerShell to install WSL:
     ```powershell
     wsl --install
     ```
   - This command will:
     - Enable WSL on your system.
     - Install the default Linux distribution (usually Ubuntu).

3. **Install Ubuntu from the Microsoft Store:**

   - Open the Microsoft Store on your Windows machine.
   - Search for **Ubuntu** (e.g., Ubuntu 22.04) and click **Install**.
   - Wait for the installation to complete.

4. **Restart your computer:**

   - If prompted, restart your system to complete the installation.

5. **Launch Ubuntu:**

   - After the installation completes, go to the Start Menu, search for **Ubuntu**, and launch it.
   - The first time you run Ubuntu, you will be prompted to create a user account and set a password for your Ubuntu system.

---

## 3. Manually Enable WSL Features (If `wsl --install` Doesn’t Work)

If the `wsl --install` command doesn’t automatically enable the necessary Windows features for WSL to work, manually enable them before restarting your system.

1. **Open PowerShell as Administrator:**

   - Right-click on the Start button, and select **Windows Terminal (Admin)** or **PowerShell (Admin)**.

2. **Manually Enable the Required Features:**

   - Enable WSL Feature:
     ```powershell
     dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
     ```
   - Enable Virtual Machine Platform Feature:
     ```powershell
     dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
     ```

3. **Optional: Enable Hyper-V (Required for WSL 2):**

   - If you plan to use WSL 2 in the future, you can also enable Hyper-V:
     ```powershell
     dism.exe /online /enable-feature /featurename:Microsoft-Hyper-V-All /all /norestart
     ```

4. **Restart Your Computer:**

   - After running the commands, restart your computer to apply the changes.

---

## 4. Change DNS to 8.8.8.8

After Ubuntu is installed in WSL, you may need to change the DNS settings to avoid potential DNS resolution issues.

1. **Navigate to the WSL File System:**

   - Open File Explorer and type the following in the address bar:
     ```
     \\wsl.localhost\Ubuntu-22.04\mnt\wsl
     ```

2. **Edit the `resolv.conf` File:**

   - Open the Ubuntu terminal and run:
     ```bash
     sudo nano /etc/resolv.conf
     ```

3. **Modify the DNS Settings:**

   - Change the `nameserver` line to:
     ```bash
     nameserver 8.8.8.8
     ```

4. **Save the Changes:**

   - Press **Ctrl + X** to exit the editor.
   - Press **Y** to confirm saving the file.
   - Press **Enter** to confirm the filename and save it.

---

## 5. Fix Untrusted Certificate Error

To resolve the Untrusted Certificate error (common with ASP.NET Core and HTTPS), follow these steps to create the necessary folders in `%APPDATA%`.

1. **Navigate to `%APPDATA%`:**

   - Open File Explorer and type the following in the address bar:
     ```
     %APPDATA%
     ```

2. **Create the Necessary Folders:**

   - Inside the `AppData\Roaming` folder, create a new folder called `ASP.NET`.
   - Inside the `ASP.NET` folder, create a new folder called `Https`.

---

## 6. Use SQLite for Database

Since WSL cannot run LocalDB, you'll need to configure your .NET 8 project to use SQLite instead. Follow these steps to set up the MockDB using SQLite:

1. **Install `Microsoft.EntityFrameworkCore.Sqlite`:**

   - In Visual Studio, go to **Tools** > **NuGet Package Manager** > **Manage NuGet Packages for Solution**.
   - In the NuGet Package Manager window, search for `Microsoft.EntityFrameworkCore.Sqlite`.
   - Click **Install** to add the SQLite provider to your project.

2. **Update `appsettings.json`:**

   - Open your `appsettings.json` file in your project.
   - Add a new connection string for SQLite:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=mockdb.db"
     }
     ```

3. **Modify `Startup.cs` or `Program.cs`:**

   - In `Startup.cs` or `Program.cs`, modify the database configuration to use SQLite instead of SQL Server:
     ```csharp
     public void ConfigureServices(IServiceCollection services)
     {
         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
     }
     ```

---

## 7. Run .NET 8 Project in Visual Studio as WSL

Now that you’ve set up WSL and SQLite, you can run your .NET 8 project in Visual Studio using WSL.

1. **Set WSL as the Target Environment:**

   - In Visual Studio, go to **Tools** > **Options** > **Cross Platform** > **Linux**.
   - Set **Ubuntu (WSL)** as the target environment.

2. **Run the Project:**

   - Build and run your project in Visual Studio. It should now run inside Ubuntu under WSL.

---

## 8. Apply EF Core Migrations to Create MockDB

To set up the MockDB for the first time, you need to apply the Entity Framework Core migrations.

1. **Open the Terminal in Visual Studio:**

   - Go to **Tools** > **NuGet Package Manager** > **Package Manager Console**.

2. **Add a Migration:**

   - Run the following command to add a migration:
     ```bash
     dotnet ef migrations add InitialCreate
     ```

3. **Update the Database:**

   - Apply the migration by running:
     ```bash
     dotnet ef database update
     ```
   - This will create your SQLite database (`MockDB`) in the project directory.

---

## 9. Setup Your Own Certificate

To set up a trusted self-signed certificate for your .NET 8 project, follow these steps:

1. **Clean Existing Certificates:**

   - Run the following command to remove any self-signed certificates:
     ```bash
     dotnet dev-certs https --clean
     ```

2. **Generate a New Self-Signed Certificate:**

   - Create a new trusted self-signed certificate:
     ```bash
     dotnet dev-certs https --trust
     ```

3. **Export the Certificate:**

   - Run the following command to export the certificate:
     ```bash
     dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\[CertificateName].pfx -p [Password]
     ```

4. **Update `appsettings.json`:**

   - Add the HTTPS configuration to your `appsettings.json`:
     ```json
     "Kestrel": {
       "Endpoints": {
         "Http": {
           "Url": "http://localhost:5056"
         },
         "Https": {
           "Url": "https://localhost:7194",
           "Certificate": {
             "Path": "",
             "Password": ""
           }
         }
       }
     }
     ```

5. **Add Password to `secrets.json`:**

   - Store the password securely in `secrets.json`:
     ```json
     {
       "KestrelPassword": "[Password]"
     }
     ```

6. **Update `Program.cs`:**

   - Dynamically load the certificate path and password from configuration:
     ```csharp
     string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
     userFolder = Path.Combine(userFolder, ".aspnet");
     userFolder = Path.Combine(userFolder, "https");
     userFolder = Path.Combine(userFolder, "[CertificateName].pfx");
     builder.Configuration.GetSection("Kestrel:EndPoints:Https:Certificate:Path").Value = userFolder;

     string kestrelPassword = builder.Configuration.GetValue<string>("KestrelPassword");
     builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate:Password").Value = kestrelPassword;
     ```

---

## 10. Run Your Project in Docker

To run your project in Docker as an alternative to Kestrel or IIS, follow these steps:

1. **Install Docker Desktop:**

   - Download and install Docker Desktop from the [Docker website](https://www.docker.com/products/docker-desktop).

2. **Add Docker Support to Your Project:**

   - Open the project in Visual Studio.
   - Right-click on the project and select **Add** > **Docker Support**.
   - In the Docker Support dialog:
     - Ensure the **Container OS** is set to **Linux**. If it is not, change it to Linux.
     - Set the **Container Build Type** to **Dockerfile**.
     - Confirm the **Docker Build Context** is the path to your project folder.
   - Click **OK** to apply the changes.

3. **Build the Docker Image:**

   - Open a terminal (Command Prompt, PowerShell, or Docker Desktop Terminal) and navigate to your project directory.
   - Run the following command to build the Docker image:
     ```bash
     docker build -t [ImageName] .
     ```

4. **Handle Certificates (If Step 9 Is Completed):**

   - If you have set up a self-signed certificate in Step 9, out-comment steps 4 and 6 in the certificate setup instructions.
   - Include the certificate parameters in the Docker run command.

5. **Run the Docker Container:**

   - Execute the following command to run the container with the self-signed certificate:
     ```bash
     docker run --name [Name] -p 8000:80 -p 8001:443 \
     -e ASPNETCORE_URLS="https://+;http://+" \
     -e ASPNETCORE_HTTPS_PORT=8001 \
     -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/[CertificateName] \
     -e ASPNETCORE_Kestrel__Certificates__Default__Password="[Password to Certificate]" \
     -e ASPNETCORE_ENVIRONMENT=Development \
     -v %USERPROFILE%\.aspnet\https:/https/ \
     [ImageName] .
     ```

   - Ensure all paths and environment variables are correctly set for your project.

---

This guide should help you set up and run your .NET 8 project using WSL, SQLite, self-signed certificates, and Docker. Enjoy coding!

