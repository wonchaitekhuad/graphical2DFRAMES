# Testing Guide: Decimal Coordinates Feature

## Overview
This document provides instructions for testing the new DecimalPointsForm feature that allows the WinForms application to accept decimal coordinates, display them with 4 decimal places, and draw them with auto-scaling.

## Build Requirements
- **Visual Studio 2019** or later
- **.NET Framework 4.7.2** or higher
- Windows operating system

## How to Build and Run

### Option 1: Using the Fixed ZIP File

1. **Download the Fixed ZIP**:
   - Download `C# VS 2019 .NET Framework 4.7.2-fixed.zip` from the repository root
   - Extract the ZIP file to a local directory

2. **Open the Solution**:
   - Navigate to the extracted directory
   - Double-click `Graphical_2D_Frame_Analysis_CSharp.sln` to open in Visual Studio

3. **Build the Project**:
   - In Visual Studio, select **Build > Rebuild Solution**
   - Wait for the build to complete successfully

4. **Run the Application**:
   - Press **F5** or click **Debug > Start Debugging**
   - The DecimalPointsForm should launch automatically

### Option 2: Using the Repository Source

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/wonchaitekhuad/graphical2DFRAMES.git
   cd graphical2DFRAMES
   git checkout feature/decimal-coordinates
   ```

2. **Open the Solution**:
   - Open `Graphical_2D_Frame_Analysis_CSharp.sln` in Visual Studio 2019

3. **Build and Run**:
   - Build the solution (Ctrl+Shift+B)
   - Run the application (F5)

## Testing the DecimalPointsForm

### Test Case 1: Adding Decimal Coordinates

1. **Input decimal coordinates**:
   - In the **X** field, enter: `10.5678`
   - In the **Y** field, enter: `20.1234`
   - Click **Add**

2. **Expected Result**:
   - The coordinate should appear in the ListBox as: `Point 1: (10.5678, 20.1234)`
   - A red dot should appear on the canvas
   - A label should show the point number and coordinates

3. **Repeat with more points**:
   - Add `X: 15.9999`, `Y: 25.0001` → Click **Add**
   - Add `X: 5.1111`, `Y: 30.2222` → Click **Add**
   - Add `X: 12.3456`, `Y: 18.7890` → Click **Add**

### Test Case 2: Locale-Specific Decimal Separator

For users with different locale settings (e.g., European format with comma as decimal separator):

1. **Test with comma separator** (if your system uses it):
   - Enter: `X: 10,5` and `Y: 20,3`
   - Click **Add**

2. **Expected Result**:
   - The application should correctly parse the input according to your system's `CultureInfo.CurrentCulture`
   - The coordinate should be added successfully

### Test Case 3: F4 Formatting (4 Decimal Places)

1. **Add coordinates with varying decimal places**:
   - Enter `X: 10`, `Y: 20` → Click **Add**
   - Enter `X: 15.1`, `Y: 25.2` → Click **Add**
   - Enter `X: 12.12345`, `Y: 18.98765` → Click **Add**

2. **Expected Result in ListBox**:
   - `Point 1: (10.0000, 20.0000)`
   - `Point 2: (15.1000, 25.2000)`
   - `Point 3: (12.1235, 18.9877)` (rounded to 4 decimal places)

### Test Case 4: Update Operation

1. **Select an existing point**:
   - Click on any point in the ListBox (e.g., Point 2)

2. **Modify the values**:
   - The X and Y fields should populate with the selected point's coordinates
   - Change `X: 99.9999` and `Y: 88.8888`
   - Click **Update**

3. **Expected Result**:
   - The selected point should update in the ListBox
   - The canvas should reflect the new position

### Test Case 5: Remove Operation

1. **Select a point**:
   - Click on any point in the ListBox

2. **Remove it**:
   - Click **Remove**

3. **Expected Result**:
   - The point should be removed from the ListBox
   - The canvas should refresh without that point

### Test Case 6: Clear All Operation

1. **Clear all coordinates**:
   - Click **Clear All**
   - Confirm the dialog

2. **Expected Result**:
   - All points should be removed from the ListBox
   - The canvas should be cleared

### Test Case 7: Auto-Scaling

1. **Add points with large coordinate ranges**:
   - Add `X: 0.0001`, `Y: 0.0001`
   - Add `X: 1000.9999`, `Y: 1000.9999`

2. **Expected Result**:
   - Both points should be visible on the canvas
   - The coordinate system should automatically scale to fit all points
   - A 10% padding should be visible around the points

3. **Add points close together**:
   - Clear all points
   - Add `X: 50.0001`, `Y: 50.0001`
   - Add `X: 50.0002`, `Y: 50.0002`

4. **Expected Result**:
   - Both points should be visible and distinguishable
   - The canvas should zoom in appropriately

### Test Case 8: Invert Y-Axis

1. **Add several points**:
   - Add at least 3-4 points with varying Y coordinates

2. **Toggle the Invert Y-Axis checkbox**:
   - Check the checkbox
   - Observe the canvas

3. **Expected Result**:
   - With checkbox **unchecked**: Y increases upward (Cartesian coordinate system)
   - With checkbox **checked**: Y increases downward (screen coordinate system)
   - Points should redraw in the new orientation

### Test Case 9: Invalid Input Handling

1. **Enter invalid data**:
   - Enter `X: abc` and `Y: 123` → Click **Add**

2. **Expected Result**:
   - A message box should appear: "Please enter valid decimal numbers for X and Y coordinates."
   - No point should be added

3. **Test empty fields**:
   - Leave X or Y empty → Click **Add**

4. **Expected Result**:
   - Same validation message should appear

### Test Case 10: Visual Verification

1. **Check the canvas drawing**:
   - Verify red dots are drawn for each point
   - Verify blue labels show point number and coordinates
   - Verify grid lines are visible in light gray
   - Verify a black border surrounds the drawing area

2. **Check ListBox formatting**:
   - All coordinates should display exactly 4 decimal places
   - Format should be: `Point N: (X.XXXX, Y.YYYY)`

## Reverting to Original Form1

To switch back to the original Form1 application:

1. **Open Program.cs**
2. **Modify the Main method**:
   ```csharp
   Application.Run(new Form1());  // Original
   // Application.Run(new DecimalPointsForm());  // Comment out test form
   ```
3. **Rebuild and run**

## Expected Sample Output

When testing with these example coordinates:
- (10.5000, 20.3000)
- (15.7500, 25.1000)
- (5.2500, 30.9000)
- (12.0000, 18.5000)

You should see:
- 4 red dots on the canvas
- 4 labeled points with coordinates
- Auto-scaled view showing all points with padding
- ListBox showing all 4 points with F4 formatting

## Troubleshooting

### Build Errors
- **Issue**: Missing .NET Framework 4.7.2
- **Solution**: Install .NET Framework 4.7.2 Developer Pack from Microsoft

### Runtime Errors
- **Issue**: DecimalPointsForm not found
- **Solution**: Ensure DecimalPointsForm.cs is included in the project and properly compiled

### Display Issues
- **Issue**: Canvas not refreshing
- **Solution**: Try resizing the window or clicking Add/Update to trigger a repaint

## Success Criteria

The feature is working correctly if:
- ✅ Decimal coordinates can be entered and parsed using current culture
- ✅ All coordinates display with exactly 4 decimal places (F4 format)
- ✅ Add, Update, Remove, and Clear operations work correctly
- ✅ Auto-scaling shows all points within the canvas
- ✅ Invert Y-Axis checkbox toggles coordinate orientation
- ✅ Invalid input is handled with appropriate error messages
- ✅ Visual representation matches coordinate data

## Additional Notes

- The original Form1 functionality remains intact
- To use the original application, simply change Program.cs as described above
- All changes are minimal and focused on the decimal coordinates feature
- The new form is completely independent and doesn't affect existing functionality
