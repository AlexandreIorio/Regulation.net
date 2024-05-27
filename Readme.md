# Regulation Project on Raspberry Pi

This project implements a regulation system based on temperature sensors and pumps controlled by PWM (Pulse Width Modulation) on a Raspberry Pi.

## Prerequisites

Before starting, make sure you have enabled the PWM channels on your Raspberry Pi as well as GPIO4 for W1 (1-Wire).

### Enabling PWM Channels

To enable PWM channels, add or modify the following lines in your `/boot/config.txt` file:

```sh
dtoverlay=pwm-2chan
```

### Enabling GPIO4 for W1

To enable GPIO4 for W1, add or modify the following lines in your `/boot/config.txt` file:

```sh
dtoverlay=w1-gpio,gpiopin=4
```

### Opening Port 5000 with `ufw`

To allow access to the API via port 5000, make sure `ufw` (Uncomplicated Firewall) is installed and activate the following rule:

1. Install `ufw` if necessary:

```sh
sudo apt-get install ufw
```

2. Enable `ufw` (if not already enabled):

```sh
sudo ufw enable
```

3. Allow traffic on port 5000:

```sh
sudo ufw allow 5000
```

## Installation

1. Clone this repository:

```sh
git clone <repository_url>
cd <repository_name>
```

2. Set up your environment:

```sh
dotnet restore
```

## Configuration

Ensure that the pumps and temperature sensors are properly connected to the GPIO pins of your Raspberry Pi as specified in the code. Connect `GPIO12` for PWM and `GPIO4` for the sensor bus.

 ![Raspberry Pi 4 GPIO Pinout](rpi-gpio.png)


## Running the Application

To start the application, use the following command:

```sh
dotnet run
```

The API will be accessible at `http://0.0.0.0:5000`.

## Using the API

Once the application is running, you can use Swagger to explore and test the various API features. Open the following address in your browser:

```sh
http://0.0.0.0:5000/swagger
```

## Debugging
The .vscode folder with `launch.json` and `tasks.json` is ready for debugging if needed.
