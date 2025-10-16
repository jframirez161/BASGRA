# How to Upload BASGRA to GitHub

Follow these steps to upload your BASGRA project to GitHub:

## Step 1: Create a GitHub Repository

1. Go to [GitHub.com](https://github.com) and sign in to your account
2. Click the **"+"** icon in the top-right corner
3. Select **"New repository"**
4. Fill in the repository details:
   - **Repository name**: `BASGRA` (or any name you prefer)
   - **Description**: "BASGRA grass growth simulation model"
   - **Public** or **Private**: Choose based on your preference
   - **DO NOT** initialize with README (we already have one)
5. Click **"Create repository"**

## Step 2: Connect Your Local Repository to GitHub

After creating the repository, GitHub will show you commands. Use these PowerShell commands in your project folder:

```powershell
# Navigate to your project folder (if not already there)
cd C:\Users\cowtr\Desktop\BASGRA-github

# Add the remote repository (replace USERNAME with your GitHub username)
git remote add origin https://github.com/USERNAME/BASGRA.git

# Verify the remote was added
git remote -v
```

## Step 3: Push Your Code to GitHub

```powershell
# Push your code to GitHub
git push -u origin master
```

You'll be prompted to enter your GitHub credentials:
- **Username**: Your GitHub username
- **Password**: Use a **Personal Access Token** (not your password)

### How to Create a Personal Access Token:

1. Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token (classic)"
3. Give it a name (e.g., "BASGRA Upload")
4. Select scopes: Check **"repo"** (full control of private repositories)
5. Click "Generate token"
6. **COPY THE TOKEN** - you won't see it again!
7. Use this token as your password when pushing

## Step 4: Verify Upload

1. Go to your GitHub repository page
2. Refresh the page
3. You should see all your files uploaded!

## Step 5: Enable GitHub Pages (Optional - Free Hosting!)

To host your BASGRA page for free on GitHub:

1. Go to your repository on GitHub
2. Click **Settings** → **Pages** (in the left sidebar)
3. Under "Source", select **"Deploy from a branch"**
4. Select branch **"master"** and folder **"/ (root)"**
5. Click **Save**
6. Wait a few minutes, then your site will be live at:
   `https://USERNAME.github.io/BASGRA/`

## Future Updates

When you make changes to your code:

```powershell
# Add your changes
git add .

# Commit with a message
git commit -m "Description of your changes"

# Push to GitHub
git push
```

## Troubleshooting

### If you get "remote origin already exists"
```powershell
git remote remove origin
git remote add origin https://github.com/USERNAME/BASGRA.git
```

### If you need to change branches
```powershell
# Rename master to main (if needed)
git branch -M main
git push -u origin main
```

### Authentication Issues
- Make sure you're using a **Personal Access Token**, not your password
- Token must have **"repo"** permissions

## Your Repository is Ready!

Your local repository is already initialized with:
- ✅ Git initialized
- ✅ Files committed
- ✅ README.md created
- ✅ .gitignore configured

You just need to:
1. Create the GitHub repository
2. Add the remote origin
3. Push your code

Good luck! 🚀
