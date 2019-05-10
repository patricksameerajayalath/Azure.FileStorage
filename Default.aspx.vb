Imports System.IO
Imports Microsoft.WindowsAzure.Storage.Auth
Imports Microsoft.WindowsAzure.Storage.Blob

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        ShowDocumentList()

    End Sub

    Private Sub ShowDocumentList()

        Dim blobContainer As CloudBlobContainer = GetStorageContainerReference()

        '<add key="storage:directoryUploadDocs" value="upload_docs" />
        Dim directoryUploadDocs As String = ConfigurationManager.AppSettings("storage:directoryUploadDocs")
        Dim blobDirectoryUploadDocs As CloudBlobDirectory = blobContainer.GetDirectoryReference(directoryUploadDocs)
        Dim blockBlobUploadDocs As CloudBlockBlob = blobContainer.GetBlockBlobReference(directoryUploadDocs)

        Dim blobList as IEnumerable(Of IListBlobItem) = blobDirectoryUploadDocs.ListBlobs()

        Try

            ContainerName.Text = blockBlobUploadDocs.Container.Name
            DirectoryPath.Text = blockBlobUploadDocs.Uri.AbsolutePath

            documentList.Text = "<table class='table table-hover'>"

            documentList.Text = documentList.Text & "<tr>"
            documentList.Text = documentList.Text & "<th scope='col'>Type</th>"
            documentList.Text = documentList.Text & "<th scope='col'>Name</th>"
            documentList.Text = documentList.Text & "<th scope='col'>Path</th>"
            documentList.Text = documentList.Text & "<th scope='col'>Absolute Path</th>"
            documentList.Text = documentList.Text & "<th scope='col'>URL</th>"
            documentList.Text = documentList.Text & "<th scope='col'>File Content</th>"
            documentList.Text = documentList.Text & "<th scope='col'>File Properties</th>"
            documentList.Text = documentList.Text & "</tr>"

            For Each blobItem As IListBlobItem In blobList

                If blobItem.GetType() = GetType(CloudBlockBlob) Then

                    Dim blob As CloudBlockBlob = DirectCast(blobItem, CloudBlockBlob)

                    documentList.Text = documentList.Text & "<tr>"

                    ' blob type
                    documentList.Text = documentList.Text & "<td>File</td>"

                    ' file name
                    Dim fileName As String = Path.GetFileName(blob.Name)
                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & fileName
                    documentList.Text = documentList.Text & "</td>"

                    ' blob path
                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & blob.Name
                    documentList.Text = documentList.Text & "</td>"
                    
                    ' blob absolute path
                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & blob.Uri.AbsolutePath
                    documentList.Text = documentList.Text & "</td>"

                    ' blob url
                    Dim policy As new SharedAccessBlobPolicy
                    policy.Permissions = SharedAccessBlobPermissions.Read
                    policy.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(20)

                    Dim blobUri as String = blob.Uri.ToString() + blob.GetSharedAccessSignature(policy)

                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & "<a href=""" + blobUri + """ >"
                    documentList.Text = documentList.Text & "View File"
                    documentList.Text = documentList.Text & "</a>"
                    documentList.Text = documentList.Text & "</td>"

                    ' blob content
                    Dim blockBlobFile As CloudBlockBlob = blobDirectoryUploadDocs.GetBlockBlobReference(fileName)
                    Dim streamReader As New StreamReader(blockBlobFile.OpenRead())

                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & "<textarea cols='20' rows='2'>"
                    documentList.Text = documentList.Text & streamReader.ReadToEnd()
                    documentList.Text = documentList.Text & "</textarea>"
                    documentList.Text = documentList.Text & "</td>"

                    ' blob properties
                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & "File size: " & String.Format("{0:#,###}", blob.Properties.Length) & " bytes <br />"
                    documentList.Text = documentList.Text & "Last modified: " & blob.Properties.LastModified.ToString
                    documentList.Text = documentList.Text & "</td>"

                    documentList.Text = documentList.Text & "</tr>"

                Else If blobItem.GetType() = GetType(CloudBlobDirectory) Then

                    Dim blob As CloudBlobDirectory = DirectCast(blobItem, CloudBlobDirectory)

                    documentList.Text = documentList.Text & "<tr>"

                    ' type
                    documentList.Text = documentList.Text & "<td>Folder</td>"

                    ' directory name
                    Dim directoryName As String = New DirectoryInfo(blob.Prefix).Name
                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & directoryName
                    documentList.Text = documentList.Text & "</td>"

                    ' blob path
                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & blob.Prefix
                    documentList.Text = documentList.Text & "</td>"

                    ' blob absolute path
                    documentList.Text = documentList.Text & "<td>"
                    documentList.Text = documentList.Text & blob.Uri.AbsolutePath
                    documentList.Text = documentList.Text & "</td>"

                    ' blob url
                    documentList.Text = documentList.Text & "<td>-</td>"

                    ' blob content
                    documentList.Text = documentList.Text & "<td>-</td>"

                    ' blob properties
                    documentList.Text = documentList.Text & "<td>-</td>"

                    documentList.Text = documentList.Text & "</tr>"

                End If

            Next

            documentList.Text = documentList.Text & "</table>"

        Catch ee As Exception

            documentList.Text = documentList.Text & "Access not possible, error: <i>"
            documentList.Text = documentList.Text & ee.ToString() + "</i>"

        End Try

    End Sub

    Private Function GetStorageContainerReference() As CloudBlobContainer

        Dim credentials As StorageCredentials = New StorageCredentials(ConfigurationManager.AppSettings("storage:accountName"), ConfigurationManager.AppSettings("storage:key"))
        Dim blobClient As CloudBlobClient = New CloudBlobClient(New Uri(ConfigurationManager.AppSettings("storage:uri")), credentials)

        Dim containerName As String = ConfigurationManager.AppSettings("storage:containerName")
        Dim blobContainer As CloudBlobContainer = blobClient.GetContainerReference(containerName)

        Return blobContainer

    End Function

    Private Sub AddFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddFile.Click

        If Page.IsPostBack = True Then

            Dim blobContainer As CloudBlobContainer = GetStorageContainerReference()

            '<add key="storage:directoryUploadDocs" value="upload_docs" />
            Dim directoryUploadDocs As String = ConfigurationManager.AppSettings("storage:directoryUploadDocs")
            Dim blobDirectoryUploadDocs As CloudBlobDirectory = blobContainer.GetDirectoryReference(directoryUploadDocs)
            Dim blockBlobUploadDocs As CloudBlockBlob = blobContainer.GetBlockBlobReference(directoryUploadDocs)

            Dim fileName As String = Path.GetFileName(FindFile.PostedFile.FileName)

            Dim selectedBlob As CloudBlockBlob = blobDirectoryUploadDocs.GetBlockBlobReference(fileName)

            If (selectedBlob.Exists()) Then

                Message.Text = "File " + fileName + " already exists."

             Else 

                Try

                    Dim sourceStream as Stream = FindFile.PostedFile.InputStream
                    sourceStream.Position = 0

                    dim cloudBlob as CloudBlockBlob = blobDirectoryUploadDocs.GetBlockBlobReference(fileName)
            
                    cloudBlob.UploadFromStream(FindFile.PostedFile.InputStream)

                    ShowDocumentList()

                    Message.Text = ""
            
                Catch err As Exception

                    Message.Text = "Error saving file " + fileName + "<br>" + err.ToString()

                End Try

            End If

        End If

    End Sub

    Private Sub DeleteFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteFile.Click

        Dim selectedFileName As String = DeleteFileName.Text

        Dim blobContainer As CloudBlobContainer = GetStorageContainerReference()

        '<add key="storage:directoryUploadDocs" value="upload_docs" />
        Dim directoryUploadDocs As String = ConfigurationManager.AppSettings("storage:directoryUploadDocs")
        Dim blobDirectoryUploadDocs As CloudBlobDirectory = blobContainer.GetDirectoryReference(directoryUploadDocs)
        Dim blockBlobUploadDocs As CloudBlockBlob = blobContainer.GetBlockBlobReference(directoryUploadDocs)

        Dim selectedBlob As CloudBlockBlob = blobDirectoryUploadDocs.GetBlockBlobReference(selectedFileName)

        If (selectedBlob.Exists()) Then

            selectedBlob.Delete()

            ShowDocumentList()

            Message.Text = ""
        
        Else 

            Message.Text = "File " + selectedFileName + " cannot be deleted. It doesn't exists."

        End If

    End Sub

    Private Sub WriteToFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles WriteToFile.Click

        Dim selectedFileName As String = WriteToFileName.Text
        Dim content As String = FileContent.Text

        Dim blobContainer As CloudBlobContainer = GetStorageContainerReference()

        '<add key="storage:directoryUploadDocs" value="upload_docs" />
        Dim directoryUploadDocs As String = ConfigurationManager.AppSettings("storage:directoryUploadDocs")
        Dim blobDirectoryUploadDocs As CloudBlobDirectory = blobContainer.GetDirectoryReference(directoryUploadDocs)
        Dim blockBlobUploadDocs As CloudBlockBlob = blobContainer.GetBlockBlobReference(directoryUploadDocs)

        Dim selectedBlob As CloudBlockBlob = blobDirectoryUploadDocs.GetBlockBlobReference(selectedFileName)

        If (selectedBlob.Exists()) Then

            Try
            
                Dim streamWriter As StreamWriter = New StreamWriter(selectedBlob.OpenWrite())

                streamWriter.Write(content)
                streamWriter.WriteLine()

                streamWriter.Close()

                ShowDocumentList()

                Message.Text = ""

            Catch ex As Exception

                Message.Text = "Error writing to file " + selectedFileName + "<br>" + err.ToString()

            End Try

        Else 

            Message.Text = "File " + selectedFileName + " doesn't exists. Content cannot be written to it."

        End If

    End Sub

    Private Sub RenameFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RenameFile.Click

        Dim oldName As String = RenameFileName.Text
        Dim newName As String = NewFileName.Text

        Dim blobContainer As CloudBlobContainer = GetStorageContainerReference()

        '<add key="storage:directoryUploadDocs" value="upload_docs" />
        Dim directoryUploadDocs As String = ConfigurationManager.AppSettings("storage:directoryUploadDocs")
        Dim blobDirectoryUploadDocs As CloudBlobDirectory = blobContainer.GetDirectoryReference(directoryUploadDocs)
        Dim blockBlobUploadDocs As CloudBlockBlob = blobContainer.GetBlockBlobReference(directoryUploadDocs)

        Dim selectedBlobOld As CloudBlockBlob = blobDirectoryUploadDocs.GetBlockBlobReference(oldName)
        Dim selectedBlobNew As CloudBlockBlob = blobDirectoryUploadDocs.GetBlockBlobReference(newName)

        If (selectedBlobOld.Exists()) Then

            Try
            
                selectedBlobNew.StartCopyFromBlob(selectedBlobOld)
                selectedBlobOld.Delete()

                ShowDocumentList()

                Message.Text = ""

            Catch ex As Exception

                Message.Text = "Error writing to file " + newName + "<br>" + err.ToString()

            End Try

        Else 

            Message.Text = "File " + oldName + " doesn't exists. Content cannot be written to it."

        End If

    End Sub

End Class