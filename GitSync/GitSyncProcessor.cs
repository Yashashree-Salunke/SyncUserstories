using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace GitSync
{
    public class GitSyncProcessor : IGitSyncProcessor
    {
        public void Execute(GitSyncContext context)
        {

            //TODO: Code to link to GIT
            //TODO: Iterate through context.Records and make changes accordingly
            //create clone

            string Username = context.Username;
            string Password = context.Password;

            //var co = new CloneOptions();
            //co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = context .Username , Password = context .Password };
            //Repository.Clone("https://github.com/Yashashree-Salunke/SyncUserstories.git", context .RepositoryUrl);
            using (var repository = new Repository(context.RepositoryUrl))
            {
                // IRepository repo = repository;
                foreach (var record in context.Records)
                {
                    List<Parameter> list = new List<Parameter>();
                    SqlConnection cons = new SqlConnection(context.SourceConnectionString);
                    SqlCommand command_one = new SqlCommand("SELECT * FROM UserStories WHERE Id =@StoryId");
                    command_one.Parameters.AddWithValue("@StoryId", record.UserstoryId);
                    SqlCommand command_Two = new SqlCommand("SELECT Title  FROM Tags JOIN UserstoryTags ON Tags.Id = UserstoryTags.TagId WHERE UserstoryTags.UserstoryId = @StoryId");
                    command_Two.Parameters.AddWithValue("@StoryId", record.UserstoryId);
                    SqlCommand command_Three = new SqlCommand("SELECT Title,Gwt FROM AcceptanceCriterias WHERE AcceptanceCriterias.UserstoryId = @StoryId");
                    command_Three.Parameters.AddWithValue("@StoryId", record.UserstoryId);
                    //SqlCommand NewCommand = new SqlCommand("SELECT Id,HasChildren,ParentId,Title FROM UserStories");
                    command_one.Connection = cons;
                    command_one.Connection.Open();
                    
                    SqlDataReader reader1 = command_one.ExecuteReader();
                   
                    Parameter p = new Parameter();
                    while (reader1.Read())
                    {
                        p.Id = (int)reader1["Id"];
                        p.HasChild = (bool)reader1["HasChildren"];

                        if (reader1["ParentId"] == DBNull.Value)
                        {
                            p.ParentId = 0;
                        }
                        else
                        {
                            p.ParentId = (int)reader1["ParentId"];
                        }
                        if (reader1["RootId"] == DBNull.Value)
                        {
                            p.RootId = 0;
                        }
                        else
                        {
                            p.RootId = (int)reader1["RootId"];
                        }
                        p.Title = (string)reader1["Title"];
                        p.Description = (string)reader1["Description"];
                        list.Add(p);
                    }
                    
                    command_one.Connection.Close();
                    command_Two.Connection = cons;
                    command_Two.Connection.Open();
                    SqlDataReader reader2 = command_Two.ExecuteReader();
                    while (reader2.Read())
                    {
                        p.TagName = (string)reader2["Title"];
                        list.Add(p);
                    }
                    command_Two.Connection.Close();            
                    command_Three.Connection = cons;
                    command_Three.Connection.Open();
                    SqlDataReader reader3 = command_Three.ExecuteReader();
                    while (reader3.Read())
                    {
                        p.ACriteriaName = (string)reader3["Title"];
                        p.GWT = (string)reader3["Gwt"];
                        list.Add(p);
                    }
                    List<Parameter> NewList = new List<Parameter>();
                    NewList.AddRange(list);

                    if (record.UserstorySyncActionTypeId == 1)
                    {
                        CreateFile();
                    }

                    if(record.UserstorySyncActionTypeId == 2)
                    {
                        foreach (var item in list)
                        {
                            if (record.UserstoryId == item.Id)
                            {
                                DirectoryInfo ParentDirectory = new DirectoryInfo(context.RepositoryUrl);
                                string FileName = (item.Title.Replace(" ", "") + ".feature");
                                var folder = ParentDirectory.GetFiles(FileName, SearchOption.AllDirectories).Select(t => t.FullName).ToList();
                                string Fullpath = folder.Single();
                                if(File .Exists (Fullpath))
                                {
                                    File.Delete(Fullpath);
                                }
                                
                            }
                        }
                    }

                    if(record.UserstorySyncActionTypeId == 3)
                    {
                        foreach (var item in list)
                        {
                            if (record.UserstoryId == item.Id)
                            {
                                DirectoryInfo ParentDirectory = new DirectoryInfo(context.RepositoryUrl);
                                string FileName =( item.Title.Replace(" ", "")+ ".feature");
                                var folder = ParentDirectory.GetFiles(FileName, SearchOption.AllDirectories).Select(t => t.FullName).ToList();
                                string Fullpath = folder.Single();
                                if (File.Exists(Fullpath))
                                {  
                                    using (StreamWriter sw = new StreamWriter(Fullpath))
                                    {
                                        sw.Write(string.Empty);
                                        Console.WriteLine("Enter The new content in a file");
                                        string Content = Console.ReadLine();
                                        sw.WriteLine(Content);
                                        sw.Close();
                                    }
                                }
                            }
                        }
                    }
                    void CreateFile()
                    {
                        DirectoryInfo ParentDirectory = new DirectoryInfo(context.RepositoryUrl);
                        foreach (var item in list)
                        {
                            if (record.UserstoryId == item.Id)
                            {
                                if (item.ParentId == 0 && item.RootId == 0)
                                {
                                    if (item.HasChild == true)
                                    {
                                        var FolderName = item.Title.Replace(" ", "_");
                                        var Folder = Directory.CreateDirectory(
                                             Path.Combine(ParentDirectory.FullName, FolderName));
                                        string FileName = item.Title.Replace(" ", "");
                                        string fullpath = Path.Combine(repository.Info.WorkingDirectory, FileName + ".feature");
                                        var contents = "Feature:" + item.Title + "\n" + item.Description + "\n" + "@" + item.TagName + "\n"
                                                 + "Scenario:" + item.ACriteriaName + "\n" + "\t" + item.GWT;
                                        File.WriteAllText(fullpath, contents.Replace('*', ' '));
                                        repository.Index.Add(FileName + ".feature");
                                        LibGit2Sharp.Commands.Stage(repository, fullpath);
                                    }
                                    else
                                    {
                                        string FileName = item.Title.Replace(" ", "");
                                        string fullpath = Path.Combine(repository.Info.WorkingDirectory, FileName + ".feature");
                                        var contents = "Feature:" + item.Title + "\n" + item.Description + "\n" + "@" + item.TagName + "\n"
                                                + "Scenario:" + item.ACriteriaName + "\n" + "\t" + item.GWT;
                                        File.WriteAllText(fullpath, contents.Replace('*', ' '));
                                        repository.Index.Add(FileName + ".feature");
                                        LibGit2Sharp.Commands.Stage(repository, fullpath);
                                    }
                                }
                                if (p.ParentId > 0)
                                {
                                    if (item.HasChild == true)
                                    {
                                        var FolderName = item.Title.Replace(" ", "_");
                                        foreach (var items in NewList)
                                        {
                                            if (items.Id == item.ParentId)
                                            {
                                                string title = items.Title;
                                                List<string> folders = ParentDirectory.GetDirectories(title.Replace(" ", "_"), SearchOption.AllDirectories).Select(t => t.FullName).ToList();
                                                string Fullpath = folders.Single();
                                                var folder = Directory.CreateDirectory(Path.Combine(Fullpath, FolderName));
                                                string file = item .Title.Replace(" ", "");
                                                string filepath = Path.Combine(Fullpath, title + ".feature");
                                                var contents = "Feature:" + item.Title + "\n" + item.Description + "\n" + "@" + item.TagName + "\n"
                                                + "Scenario:" + item.ACriteriaName + "\n" + "\t" + item.GWT;
                                                File.WriteAllText(filepath, contents.Replace('*', ' '));
                                                repository.Index.Add(title + ".feature");
                                                LibGit2Sharp.Commands.Stage(repository, filepath);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        string Filename = item.Title.Replace(" ", "");
                                        foreach (var items in NewList)
                                        {
                                            if (items.Id == item.ParentId)
                                            {
                                                
                                                string title = items.Title;
                                                List<string> folders = ParentDirectory.GetDirectories(title.Replace(" ", "_"), SearchOption.AllDirectories).Select(t => t.FullName).ToList();
                                                string Fullpath = folders.Single();
                                                string newpath = Path.Combine(Fullpath, Filename + ".feature");
                                                int fileIndex = repository.Info.WorkingDirectory.Count();
                                                string file = newpath.Remove(0, fileIndex);
                                                var contents = "Feature:" + item .Title + "\n" + item.Description + "\n" + "@" + item .TagName + "\n"
                                                + "Scenario:" + item.ACriteriaName + "\n" + "\t" + item .GWT;
                                                File.WriteAllText(newpath, contents.Replace('*', ' '));
                                                repository.Index.Add(file);
                                                LibGit2Sharp.Commands.Stage(repository, newpath);
                                            }
                                        }
                                    }
                                }

                            }
                        }

                    }
                }
            }

        }


    }
}
