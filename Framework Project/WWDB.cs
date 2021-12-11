﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Project_A
{
    public class WWDB
    {
        private string pg_sign_data = "Host=localhost;Username=postgres;Password=admin123;Database=GuitarLessons";
        private NpgsqlConnection con;

        public WWDB()
        {
            con = new NpgsqlConnection(pg_sign_data);
            con.Open();
        }
        public string TestConnection()
        {
            string com = "SELECT version()";
            var cmd = new NpgsqlCommand(com, con);

            var version = cmd.ExecuteScalar().ToString();
            return $"PostgreSQL version: {version}";
        }
        public Chord GetChord(string ChordName)
        {
            Dictionary<int, int?> ChordPosition = new Dictionary<int, int?> { [1] = null, [2] = null, [3] = null, [4] = null, [5] = null, [6] = null };
            int? bare = null;

            string com = "select * from chords where chordname = '" + ChordName + "'";
            var cmd = new NpgsqlCommand(com, con);
            NpgsqlDataReader Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        ChordPosition[i + 1] = Reader.GetInt32(i);
                    }
                    catch
                    {
                        ChordPosition[i + 1] = null;
                    }
                }
                try
                {
                    bare = Reader.GetInt32(6);
                    Reader.Close();
                    return new Chord(ChordName, ChordPosition, (int)bare);
                }
                catch
                {
                    Reader.Close();
                    return new Chord(ChordName, ChordPosition);
                }
            }
            Reader.Close();
            return new Chord();
        }
        public List<Chord> GetChordsList()
        {
            List<string> chordsnames = new List<string>();
            string com = "select chordname from chords";
            var cmd = new NpgsqlCommand(com, con);
            NpgsqlDataReader Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                chordsnames.Add(Reader.GetString(0));
            }
            Reader.Close();

            List<Chord> returnList = new List<Chord>();
            foreach (string chordname in chordsnames)
            {
                returnList.Add(GetChord(chordname));
            }

            return returnList;
        }

        public List<Dictionary<string, string>> GetSongsList()
        {
            List<Dictionary<string, string>> songslist = new List<Dictionary<string, string>>();

            string com = "select name, artist from songs";
            var cmd = new NpgsqlCommand(com, con);
            NpgsqlDataReader Reader = cmd.ExecuteReader();

            while (Reader.Read())
            {
                songslist.Add( new Dictionary<string, string> { ["name"] = Reader.GetString(0), ["artist"] = Reader.GetString(1) });
            }

            return songslist;
        }
        public string GetSongData(int id)
        {
            string songdata = "Данных нет";
            string com = string.Format("select songdata from songs where id = {0}", id);
            var cmd = new NpgsqlCommand(com, con);
            NpgsqlDataReader Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                songdata = Reader[0].ToString();
            }
            return songdata;
        }
        public string GetSongData(string name, string artist)
        {
            string songdata = "Данных нет";
            string com = string.Format("select songdata from songs where name = '{0}' and artist = '{1}'", name, artist);
            var cmd = new NpgsqlCommand(com, con);
            NpgsqlDataReader Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                songdata = Reader[0].ToString();
            }
            return songdata;
        }
    }
}
