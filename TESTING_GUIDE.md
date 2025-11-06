# DecimalPointsForm Testing Guide

## Overview
DecimalPointsForm is a new WinForms component that supports decimal coordinates with:
- Float data type (PointF)
- F4 formatting (4 decimal places)
- Auto-scaling visualization
- CultureInfo.CurrentCulture support for both '.' and ',' decimal separators

## Features Implemented

### 1. Input Fields
- X and Y TextBox controls for coordinate entry
- Uses `float.TryParse(..., NumberStyles.Float, CultureInfo.CurrentCulture)` for parsing
- Supports both '.' and ',' as decimal separators based on system locale

### 2. ListBox Display
- Shows all coordinates in F4 format: `(X.XXXX, Y.YYYY)`
- Format string: `string.Format(CultureInfo.CurrentCulture, "({0:F4}, {1:F4})", point.X, point.Y)`

### 3. Buttons
- **Add**: Adds new coordinate to the list
- **Update**: Updates selected coordinate
- **Remove**: Removes selected coordinate
- **Clear**: Clears all coordinates

### 4. PictureBox Visualization
- Auto-scaling: Maps world coordinates to screen coordinates
- 10% padding margin around all points
- Draws coordinate axes if they fall within the visible range
- Each point is drawn as a red circle with label showing F4 formatted coordinates

### 5. Y-Axis Inversion
- CheckBox control to toggle between:
  - Standard coordinate system (Y increases upward)
  - Screen coordinate system (Y increases downward)

## How to Test

### Sample Test Values
Enter these coordinates to verify F4 formatting and auto-scaling:

1. First point: `12.3456, 78.9012`
2. Second point: `-5.6789, 45.1234`
3. Third point: `100.0001, -20.5555`
4. Fourth point: `0.0000, 0.0000`

### Expected Behavior

1. **ListBox Display**: 
   - Should show: `(12.3456, 78.9012)`
   - Should show: `(-5.6789, 45.1234)`
   - Should show: `(100.0001, -20.5555)`
   - Should show: `(0.0000, 0.0000)`

2. **PictureBox**:
   - All points should be visible
   - Points should auto-scale to fill the canvas
   - Labels should display F4 formatted coordinates next to each point
   - Coordinate axes should appear if they intersect the data range

3. **Y-Axis Inversion**:
   - Unchecked: Y increases downward (standard screen coordinates)
   - Checked: Y increases upward (mathematical coordinates)

4. **Culture Support**:
   - On systems with comma decimal separator (e.g., German), enter: `12,3456` and `78,9012`
   - On systems with period decimal separator (e.g., English), enter: `12.3456` and `78.9012`
   - Both should work correctly

### Update Program.cs
The Program.cs has been temporarily modified to launch DecimalPointsForm:
```csharp
Application.Run(new DecimalPointsForm());
```

To revert to the original form:
```csharp
Application.Run(new Form1());
```

## Build Instructions

### On Windows with Visual Studio 2019 or later:
1. Extract `C# VS 2019 .NET Framework 4.7.2-fixed.zip`
2. Open `Graphical_2D_Frame_Analysis_CSharp.sln`
3. Build Solution (Ctrl+Shift+B)
4. Run (F5)

### Expected Result:
- Application should launch showing the DecimalPointsForm
- Enter test coordinates and verify F4 formatting in ListBox
- Verify points are drawn on PictureBox with auto-scaling
- Test Y-axis inversion checkbox
- Test Add/Update/Remove/Clear operations

## Code Changes Summary

### Files Modified:
1. **DecimalPointsForm.cs** (NEW): Complete implementation of decimal coordinate management
2. **Program.cs**: Changed to run DecimalPointsForm instead of Form1
3. **Graphical_2D_Frame_Analysis_CSharp.csproj**: Added DecimalPointsForm.cs compilation entry

### Key Implementation Details:
- Uses `List<PointF>` to store coordinates
- `float.TryParse` with `CultureInfo.CurrentCulture` for locale-aware parsing
- `String.Format` with F4 format specifier for consistent 4-decimal display
- Auto-scaling algorithm calculates min/max bounds and applies appropriate scale factors
- Y-axis inversion uses different mapping formulas in the Paint event handler
