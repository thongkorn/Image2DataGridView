#Region "ABOUT"
' / --------------------------------------------------------------------------------
' / Developer : Mr.Surapon Yodsanga (Thongkorn Tubtimkrob)
' / eMail : thongkorn@hotmail.com
' / URL: http://www.g2gnet.com (Khon Kaen - Thailand)
' / Facebook: https://www.facebook.com/g2gnet (For Thailand)
' / Facebook: https://www.facebook.com/commonindy (Worldwide)
' / More Info: http://www.g2gsoft.com
' /
' / Purpose: Put image into the cells of DataGridView @Runtime.
' / Microsoft Visual Basic .NET (2010)
' /
' / This is open source code under @CopyLeft by Thongkorn Tubtimkrob.
' / You can modify and/or distribute without to inform the developer.
' / --------------------------------------------------------------------------------
#End Region
Imports System.IO
Imports System.Globalization

Public Class frmImage2DataGrid
    Dim strPath As String = MyPath(Application.StartupPath)

    Private Sub frmImage2DataGrid_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '// Initialize DataGridView Control
        With DataGridView1
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoResizeColumns()
            .AllowUserToResizeColumns = True
            .AllowUserToResizeRows = True
        End With
        DataGridView1.RowTemplate.Height = 120
        '// Declare columns type.
        Dim Column1 As New DataGridViewTextBoxColumn()
        Dim Column2 As New DataGridViewTextBoxColumn()
        '// Add new Columns
        DataGridView1.Columns.AddRange(New DataGridViewColumn() { _
                Column1, Column2 _
                })
        With DataGridView1
            .Columns(0).Name = "Product ID"
            .Columns(1).Name = "Product Name"
        End With
        Dim row As String() = New String() {"1", "Product 1"}
        DataGridView1.Rows.Add(row)
        '// Add 3th column (Index = 2), It's Image.
        Dim imgCol As New DataGridViewImageColumn()
        Dim img As Image = Image.FromFile(strPath & "Blank.gif")
        With imgCol
            .Image = img
            .HeaderText = "Image"
            .Name = "img"
            .ImageLayout = DataGridViewImageCellLayout.Stretch
            .Width = 120
        End With
        DataGridView1.Columns.Add(imgCol)
        '//
        '// Add 4th column (Index = 3), Show image path.
        Dim PicturePath As New DataGridViewTextBoxColumn()
        DataGridView1.Columns.Add(PicturePath)
        With PicturePath
            .HeaderText = "Image Path"
            .ReadOnly = True
            '// Normally have to hide this columns.
            .Visible = True
        End With
        '// Add 5th column (Index = 4), Show image path.
        Dim PictureName As New DataGridViewTextBoxColumn()
        DataGridView1.Columns.Add(PictureName)
        With PictureName
            .HeaderText = "Picture Name"
            .ReadOnly = True
            .Visible = True
        End With
        Me.DataGridView1.Focus()
    End Sub

    Private Sub btnBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnBrowse.Click
        If Me.DataGridView1.Rows.Count = 0 Then Return
        Call BrowseImage()
    End Sub

    Private Sub btnAddRow_Click(sender As System.Object, e As System.EventArgs) Handles btnAddRow.Click
        Call AddRow()
    End Sub

    Private Sub btnRemoveRow_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveRow.Click
        Call RemoveRow()
    End Sub

    Private Sub AddRow()
        Dim row As String()
        If Me.DataGridView1.RowCount = 0 Then
            row = New String() {"1", "Product 1"}
            DataGridView1.Rows.Add(row)
        Else
            Dim CellValue As String = DataGridView1.Rows(DataGridView1.RowCount - 1).Cells(0).Value + 1
            row = New String() {CellValue, "Product " & CellValue}
            DataGridView1.Rows.Add(row)
        End If
        Me.DataGridView1.Focus()
        SendKeys.Send("^{HOME}")
        SendKeys.Send("^{DOWN}")
    End Sub

    Private Sub RemoveRow()
        If DataGridView1.RowCount = 0 Then Return
        '// Delete current row from DataGridView1
        DataGridView1.Rows.Remove(DataGridView1.CurrentRow)
    End Sub

    Private Sub BrowseImage()
        Dim dlgImage As OpenFileDialog = New OpenFileDialog()
        ' / Open File Dialog
        With dlgImage
            '.InitialDirectory = strPath
            .Title = "Select images"
            .Filter = "Images types (*.jpg;*.png;*.gif;*.bmp)|*.jpg;*.png;*.gif;*.bmp"
            .FilterIndex = 1
            .RestoreDirectory = True
        End With
        ' Select OK after Browse ...
        If dlgImage.ShowDialog() = DialogResult.OK Then
            Using FS As IO.FileStream = File.Open(dlgImage.FileName, FileMode.Open)
                Dim bitmap As Bitmap = New Bitmap(FS)
                Dim currentPicture As Image = CType(bitmap, Image)
                Me.DataGridView1.CurrentRow.Cells(2).Value = currentPicture
            End Using
            '// Keep Image Path.
            Me.DataGridView1.CurrentRow.Cells(3).Value = dlgImage.FileName
            '// Get the Filename + Extension only
            Dim iArr() As String
            iArr = Split(dlgImage.FileName, "\")
            '// Get highest value of array. (Upper Bound)
            Me.DataGridView1.CurrentRow.Cells(4).Value = iArr(UBound(iArr))
        End If

    End Sub

    ' / --------------------------------------------------------------------------------
    ' / Get my project path
    ' / AppPath = C:\My Project\bin\debug
    ' / Replace "\bin\debug" with "\"
    ' / Return : C:\My Project\
    Function MyPath(ByVal AppPath As String) As String
        '/ MessageBox.Show(AppPath);
        AppPath = AppPath.ToLower()
        '/ Return Value
        MyPath = AppPath.Replace("\bin\debug", "\").Replace("\bin\release", "\").Replace("\bin\x86\debug", "\").Replace("\bin\x86\release", "\")
        '// If not found folder then put the \ (BackSlash ASCII Code = 92) at the end.
        If Microsoft.VisualBasic.Right(MyPath, 1) <> Chr(92) Then MyPath = MyPath & Chr(92)
    End Function

    Private Sub btnExit_Click(sender As System.Object, e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmImage2DataGrid_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
        GC.SuppressFinalize(Me)
        Application.Exit()
    End Sub

    Private Sub DataGridView1_RowPostPaint(sender As Object, e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DataGridView1.RowPostPaint
        '// Display line numbers in the DataGridView
        Using b As SolidBrush = New SolidBrush(Me.DataGridView1.RowHeadersDefaultCellStyle.ForeColor)
            e.Graphics.DrawString(Convert.ToString(e.RowIndex + 1, CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4)
        End Using
    End Sub
End Class
