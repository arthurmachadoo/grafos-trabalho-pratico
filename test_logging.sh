#!/bin/bash
# Test script to automatically run the program and test logging

# Navigate to project directory
cd /Users/arthur/RiderProjects/grafos-rp

# Clean any existing logs file
rm -f logs.txt

# Build the project
echo "Building project..."
dotnet build grafos-rp/grafos-rp.csproj > /dev/null 2>&1

# Run the program with automated inputs:
# - Select route 1
# - Choose option 1 (Display graph)
# - Exit with any other number
echo "Running program with automated test inputs..."
echo -e "1\n1\n0" | dotnet run --project grafos-rp/grafos-rp.csproj --no-build 2>&1

# Check if logs.txt was created
if [ -f "logs.txt" ]; then
    echo ""
    echo "✅ SUCCESS: logs.txt file was created!"
    echo ""
    echo "--- Preview of logs.txt (first 30 lines) ---"
    head -n 30 logs.txt
    echo ""
    echo "--- File info ---"
    wc -l logs.txt
    ls -lh logs.txt
    echo ""
    echo "✅ Logging system is working correctly!"
else
    echo ""
    echo "❌ ERROR: logs.txt file was NOT created!"
    exit 1
fi
