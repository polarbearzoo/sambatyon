/*****************************************************************************************
 *  p2p-player
 *  An audio player developed in C# based on a shared base to obtain the music from.
 * 
 *  Copyright (C) 2010-2011 Dario Mazza, Sebastiano Merlino
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *  
 *  Dario Mazza (dariomzz@gmail.com)
 *  Sebastiano Merlino (etr@pensieroartificiale.com)
 *  Full Source and Documentation available on Google Code Project "p2p-player", 
 *  see <http://code.google.com/p/p2p-player/>
 *
 ******************************************************************************************/

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistence;
using System.Reflection;
using Persistence.Tag;

namespace Examples
{
    /// <summary>
    /// Example Module for kademlia-specific repository.
    /// </summary>
    class KademliaRepositoryExamples
    {
        private static KademliaRepository _repository=null;
        private static CompleteTag tag = new CompleteTag(@"..\..\Resource\Garden.mp3");
        /// <summary>
        /// Access method that will be executed by <c>ProgrammingExamples</c> and runs all the example method of the set.
        /// </summary>
        public static void RunExamples()
        {
            ExampleHelper.ExampleSetPrint("Kademlia Repository Examples", typeof(KademliaRepositoryExamples));
            RepositoryConfiguration conf=new RepositoryConfiguration(new {data_dir = @"..\..\Resource\Database"});
            _repository = new KademliaRepository("Raven", conf);
            CleanTagExample();
            StoreExample();
            PutExample();
            GetAllExample();
            MiscGetAndContainsExample();
            SearchExample();
            RefreshExample();
            ExpireExample();
            //DeleteExample();
        }
        /// <summary>
        /// This test the <c>Expire</c> method of the repository
        /// </summary>
        private static void ExpireExample()
        {
            ExampleHelper.ExampleMethodPrint("Clean Expire Entity", MethodInfo.GetCurrentMethod());
            _repository.Expire();
        }
        /// <summary>
        /// This example run <c>ContainsTag</c>, <c>ContainsUrl</c> and <c>GetPublicationTime</c> methods of the repository
        /// </summary>
        private static void MiscGetAndContainsExample()
        {
            ExampleHelper.ExampleMethodPrint("Show how some minor function of Get and Contains works", MethodInfo.GetCurrentMethod());
            bool resp = _repository.ContainsTag(tag.TagHash);
            Console.WriteLine("Contains Tag " + tag.TagHash + " ? " + resp);
            resp = _repository.ContainsTag("jasdkasjds");
            Console.WriteLine("Contains Tag jasdkasjds ? " + resp);
            Uri url = new Uri("http://localhost:18292");
            resp = _repository.ContainsUrl(tag.TagHash, url);
            Console.WriteLine("Resource " + tag.TagHash + " contains Url http://localhost:18292 ? " + resp);
            DateTime pubTime = _repository.GetPublicationTime(tag.TagHash, url);
            Console.WriteLine("Publication Time for Url " + url.ToString() + " on Resource " + tag.TagHash + " is " + pubTime);
        }
        /// <summary>
        /// This example shows how to use the <c>Put</c> operation to insert new Dht element into an existing Resource
        /// </summary>
        private static void PutExample()
        {
            ExampleHelper.ExampleMethodPrint("Put a new DhtElement in a Resource", MethodInfo.GetCurrentMethod());
            _repository.Put(tag.TagHash, new Uri("http://127.0.0.1:18181"), DateTime.Now.AddDays(-1).AddHours(-1));
        }
        /// <summary>
        /// This example use the <c>GetAll</c> method to show all object contained in the repository
        /// </summary>
        private static void GetAllExample()
        {
            ExampleHelper.ExampleMethodPrint("Print all KademliaResource in the repository", MethodInfo.GetCurrentMethod());
            LinkedList<KademliaResource> coll = _repository.GetAllElements();
            foreach (KademliaResource res in coll)
            {
                ExampleHelper.DumpObjectProperties(res);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// This example shows how the repository discard semanticless words.
        /// </summary>
        public static void CleanTagExample()
        {
            ExampleHelper.ExampleMethodPrint("Clean a string from semanticless words", MethodInfo.GetCurrentMethod());
            string example1 = "The wind of change";
            Console.WriteLine("\""+example1+ "\" => \""+_repository.DiscardSemanticlessWords(example1)+"\"");
        }
        /// <summary>
        /// This example uses the <c>Store</c> method to insert a new resource in the repository
        /// </summary>
        public static void StoreExample()
        {
            ExampleHelper.ExampleMethodPrint("Store a Tag and linking it with a peer URI", MethodInfo.GetCurrentMethod());
            _repository.StoreResource(tag, new Uri("http://localhost:18292"),DateTime.Now);
            CompleteTag anotherTag = new CompleteTag(@"..\..\Resource\SevenMP3.mp3");
            _repository.StoreResource(anotherTag,new Uri("http://localhost:28182"),DateTime.Now);
        }
        /// <summary>
        /// This example show how to perform research inside the repository
        /// </summary>
        public static void SearchExample()
        {
            ExampleHelper.ExampleMethodPrint("Trying to Search for Tags with a search Query", MethodInfo.GetCurrentMethod());
            string query = "cross";
            KademliaResource[] results = _repository.SearchFor(query);
            foreach (KademliaResource rs in results)
            {
                ExampleHelper.DumpObjectProperties(rs);
            }
        }
        /// <summary>
        /// This example uses the <c>DeleteTag</c> method to delete a tag from the repository
        /// </summary>
        public static void DeleteExample()
        {
            ExampleHelper.ExampleMethodPrint("Delete a previously loaded tag", MethodInfo.GetCurrentMethod());
            Console.WriteLine("Delete Result: "+_repository.DeleteTag(tag.TagHash));
        }
        /// <summary>
        /// This example shows how to refreash a resource inside the repository.
        /// </summary>
        public static void RefreshExample()
        {
            ExampleHelper.ExampleMethodPrint("Refresh a previously loaded tag", MethodInfo.GetCurrentMethod());
            _repository.RefreshResource(tag.TagHash, new Uri("http://localhost:18292"), DateTime.Now.AddHours(1));
            KademliaResource rs = _repository.Get(tag.TagHash);
            ExampleHelper.DumpObjectProperties(rs);
        }
    }
}
