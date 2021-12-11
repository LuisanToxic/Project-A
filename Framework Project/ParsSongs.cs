﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;

namespace Project_A
{
    class ParsSong
    {
        public ParsSong(string Song)
        {
            Get_Songs(Song);
        }
        public List<Song> Songs { get; set; }
        public void Get_Songs(string Song)
        {

            string url = "http://amdm.ru/search/?q=" + Song;

            List<string> SongLinks = new List<string>();
            Songs = new List<Song>();
            try { 
                string HTMLText = new WebClient().DownloadString(url);
                //Регулярное выражение для поиска A тега
                if (HTMLText.IndexOf("<tr>") != -1)
                {
                    string[] InTags = HTMLText.Split(new string[] { "<tr>" }, StringSplitOptions.None);
                    for (int i = 1; i < InTags.Length; i++)
                    {
                        SongLinks.Add(InTags[i].Split(new string[] { "</tr>" }, StringSplitOptions.None)[0]);
                    }


                    for (int i = 0; i < SongLinks.Count; i++)
                    {
                        Songs.Add(new Song());
                        string[] Resorses = SongLinks[i].Split(new string[] { "<a href=" + '\"' }, StringSplitOptions.None);
                        byte[] bytes = Encoding.Default.GetBytes(Resorses[2].Split(new string[] { "artist" + '\"' + ">" }, StringSplitOptions.None)[1].Split(new string[] { "</a>" }, StringSplitOptions.None)[0]);
                        Songs[i].Author = Encoding.UTF8.GetString(bytes);
                        Songs[i].Url = "http:" + Resorses[3].Split(new string[] { '\"' + " class" }, StringSplitOptions.None)[0];
                        bytes = Encoding.Default.GetBytes(Resorses[3].Split(new string[] { "artist" + '\"' + ">" }, StringSplitOptions.None)[1].Split(new string[] { "</a>" }, StringSplitOptions.None)[0]);
                        Songs[i].Name = Encoding.UTF8.GetString(bytes);
                        Songs[i].Get_Song();
                    }
                }
            }
            catch
            {

            }
        }
    }
    class Song
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string Full_info { get; set; }

        public void Get_Song()
        {
            Full_info = Name + "\n" + Author;
            try { 
                string HTMLText = new WebClient().DownloadString(Url);
            

            if (HTMLText.IndexOf("<pre itemprop=" + '\"' + "chordsBlock" + '\"' + ">") != -1)
            {
                string text = HTMLText.Split(new string[] { "<pre itemprop=" + '\"' + "chordsBlock" + '\"' + ">" }, StringSplitOptions.None)[1].Split(new string[] { "</pre>" }, StringSplitOptions.None)[0];
                string[] tepm = text.Split(new string[] { "<b>", "</b>" }, StringSplitOptions.None);
                text = "";
                for (int i = 0; i < tepm.Length; i++)
                {
                    text += tepm[i];
                }
                byte[] bytes = Encoding.Default.GetBytes(text);
                Text = Encoding.UTF8.GetString(bytes);


            }
            }
            catch { }
        }
        public void Print()
        {
            Console.WriteLine(Name);
            Console.WriteLine(Author);
            Console.WriteLine(Url);
            Console.WriteLine(Text);
            Console.ReadKey();
        }
    }
}