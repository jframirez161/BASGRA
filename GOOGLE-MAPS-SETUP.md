# Google Maps API Setup Guide

## Why You See "For development purposes only"

This watermark appears because the Google Maps API requires a valid API key for production use.

## How to Get Your API Key (FREE)

### Step 1: Create a Google Cloud Project

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Sign in with your Google account
3. Click **"Select a project"** → **"New Project"**
4. Name your project (e.g., "BASGRA Map")
5. Click **"Create"**

### Step 2: Enable Maps JavaScript API

1. In the Cloud Console, go to **"APIs & Services"** → **"Library"**
2. Search for **"Maps JavaScript API"**
3. Click on it and press **"Enable"**

### Step 3: Create API Key

1. Go to **"APIs & Services"** → **"Credentials"**
2. Click **"+ CREATE CREDENTIALS"** → **"API Key"**
3. Your API key will be created and displayed
4. **COPY THIS KEY** - you'll need it!

### Step 4: Restrict Your API Key (Recommended for Security)

1. Click on your new API key to edit it
2. Under **"Application restrictions"**:
   - Select **"HTTP referrers (websites)"**
   - Add your website domain (e.g., `https://yourusername.github.io/*`)
   - For local testing, add: `http://localhost:*` and `http://127.0.0.1:*`
3. Under **"API restrictions"**:
   - Select **"Restrict key"**
   - Choose **"Maps JavaScript API"**
4. Click **"Save"**

### Step 5: Add Your API Key to index.html

Find this line in your `index.html` file (around line 154):

```html
<script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY_HERE&v=3.exp"></script>
```

Replace `YOUR_API_KEY_HERE` with your actual API key:

```html
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx&v=3.exp"></script>
```

### Step 6: Test Your Map

1. Save the file
2. Refresh your browser
3. The "For development purposes only" message should be gone!

## Important Notes

### Free Tier Limits
- Google Maps gives you **$200 free credit per month**
- This covers approximately **28,000 map loads per month**
- For most small projects, this is completely FREE

### Billing Setup
- Google requires you to enable billing even for free usage
- You won't be charged unless you exceed the free tier
- You can set spending limits to prevent unexpected charges

### Security Best Practices

⚠️ **NEVER commit your API key to public GitHub repositories!**

Instead, use environment variables or restricted domains:

**For GitHub Pages:**
- Restrict your API key to your GitHub Pages domain: `https://yourusername.github.io/*`

**For local development:**
- Add `http://localhost:*` to allowed referrers

## Troubleshooting

### Map still shows "For development purposes only"
- Clear your browser cache
- Wait a few minutes after setting up (API activation can take 5 minutes)
- Check that your API key is correctly copied

### "This page can't load Google Maps correctly"
- Make sure billing is enabled in Google Cloud Console
- Verify the Maps JavaScript API is enabled
- Check that your API key restrictions allow your domain

### Map is grey or blank
- Check browser console for errors (F12)
- Verify your API key is valid
- Ensure your website domain matches the API key restrictions

## Cost Management

To monitor your usage:
1. Go to Google Cloud Console
2. Select your project
3. Navigate to **"APIs & Services"** → **"Dashboard"**
4. View your Maps API usage

To set a budget alert:
1. Go to **"Billing"** → **"Budgets & alerts"**
2. Create a budget (e.g., $10/month)
3. Set email alerts

## Need Help?

- [Google Maps JavaScript API Documentation](https://developers.google.com/maps/documentation/javascript/overview)
- [Google Cloud Console](https://console.cloud.google.com/)
- [Pricing Calculator](https://mapsplatform.google.com/pricing/)

---

**Remember:** Once you add your API key, commit the changes to git before pushing to GitHub!

```bash
git add index.html
git commit -m "Add Google Maps API key placeholder"
git push
```
