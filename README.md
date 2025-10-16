# BASGRA - Grass Growth Simulation Model

A web-based implementation of the BASGRA (BASic GRAss) model for simulating grass growth and predicting biomass production.

## Description

BASGRA is a scientific model that estimates grass production based on environmental conditions. This tool allows you to:

- Define a specific geographical area using Google Maps
- Input climate data (temperature, humidity, precipitation, wind, radiation)
- Calculate grass biomass production (leaves, dead leaves, and stems)
- Visualize growth patterns over time

## Features

- **Interactive Map**: Select your area of interest using Google Maps
- **Climate Data Input**: Enter daily weather data for accurate predictions
- **Real-time Calculations**: Instant biomass estimation in kg of dry matter
- **Visual Charts**: Graph showing temporal evolution of grass components
- **User-friendly Interface**: Simple data entry and calculation process

## How to Use

1. **Select Area**: Use the map to define your area of interest
2. **Enter Climate Data**: Input daily climate information in the provided table:
   - Day
   - Maximum Temperature (°C)
   - Minimum Temperature (°C)
   - Relative Humidity (%)
   - Precipitation (mm)
   - Wind Speed (m/s)
   - Solar Radiation (MJ/m²/day)
3. **Calculate**: Click the "Calcular" button to run the simulation
4. **View Results**: See the estimated biomass and growth charts

## Running the Application

This is a static web application that can be run with any web server.

### Using Node.js:
```bash
npx http-server -p 8080
```

### Using Python:
```bash
python -m http.server 8080
```

Then open your browser to `http://localhost:8080`

## Technical Stack

- HTML5/CSS3
- JavaScript
- jQuery
- FLOT Charts for data visualization
- Google Maps API for area selection
- Bootstrap for responsive design

## Requirements

- Modern web browser (Chrome, Firefox, Safari, Edge)
- Internet connection (for Google Maps API)

## Credits

- **BASGRA Model**: Based on the scientific grass growth model
- **GPS Tools**: Data capture functionality by [Jeff Baker](http://www.seabreezecomputers.com/excel2array/)
- **Area Calculator**: [Atterbury Consultants, Inc.](http://www.atterbury.com)
- **Charts**: [FLOT Charts](http://www.flotcharts.org/) by IOLA and Ole Laursen
- **Web Template**: Sadaka by [Ouarmedia](http://www.ouarmedia.com)

## License

See LICENSE file for details.

## Contributing

This is an academic tool developed at Universidad de Antioquia. For questions or contributions, please contact the development team.
