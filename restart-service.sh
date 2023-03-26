#!/bin/bash

# Stop the service
systemctl --user stop house-dashboard-server

# Wait for the service to stop
while systemctl --user is-active --quiet house-dashboard-server; do
  sleep 1
done

# Run the command
dotnet publish -c Release -o ~/web/house-dashboard-server

# Start the service
systemctl --user start house-dashboard-server
