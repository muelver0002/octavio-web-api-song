﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using SharpDevelopWebApi.Models;
namespace SharpDevelopWebApi.Controllers
{
	/// <summary>
	/// Description of SongController.
	/// </summary>
	public class SongController :ApiController
	{
		SDWebApiDbContext _db = new SDWebApiDbContext();
		
			
		
//		[HttpGet]
//		 public IHttpActionResult GetAll(string keyword = "")
//        {
//            keyword = keyword.Trim();
//            var song = new List<Song>();
//            
//            if(!string.IsNullOrEmpty(keyword))
//            {
//                song = _db.Songs
//                    .Where(x => x.Title.Contains(keyword) || x.Artist.Contains(keyword))
//                    .ToList();
//            }
//            else
//            	song = _db.Songs.ToList();
//                return Ok(song);
//        }
		 
		 
		 //Get all records Method//
		[HttpGet]
		public IHttpActionResult GetAll(string Search="", string Artist="", int? year = null, int? peak = null)
		{
			List<Song> songs;
		
			if(string.IsNullOrWhiteSpace(Search))
			{
				 songs = _db.Songs.ToList();
				
			  
			}
			else{
			
				songs = _db.Songs.Where(x => x.Title.Contains(Search) || x.Artist.Contains(Search))
				.OrderBy(o => o.Title).ToList();
				
				
			}
			
			if(!string.IsNullOrWhiteSpace(Artist))
			{
			
				songs = songs.Where(x => x.Artist.ToLower().Contains(Artist.ToLower())).ToList();
			}
				
			if(year != null)
			{
				songs = songs.Where(x => x.ReleaseYear == year.Value).ToList();
			}
			
			if(peak != null)
			{
			
				songs = songs.Where(x => x.PeakChartPosition <= peak.Value).ToList();
			}
	        
			
			int TotalCount = songs.Count();
			return Ok(new {TotalCount , songs});
		}
		
		
		
		 //Create Method//
		public IHttpActionResult Create([FromBody]Song song)
		{
			_db.Songs.Add(song);
			_db.SaveChanges();
			return Ok(song.Id);
			
			
		}
		
		
		//Update Method//
		[HttpPut]
		public IHttpActionResult Update(Song updatedSong)
		{
			var song = _db.Songs.Find(updatedSong.Id);
			
			if(song !=null)
			{
			song.Title = updatedSong.Title;
			song.Artist = updatedSong.Artist;
			song.Genre=updatedSong.Genre;
			song.ReleaseYear = updatedSong.ReleaseYear;
			song.PeakChartPosition = updatedSong.PeakChartPosition;
			_db.Entry(song).State = System.Data.Entity.EntityState.Modified;
			_db.SaveChanges();
			return Ok(song);
			}
			else
				return BadRequest("Song not found!");
		}
		
		
		
		//Delete Method//
		[Route("api/song/{id}")]
		[HttpDelete]
		public IHttpActionResult Delete(int Id)
		{
			var song = _db.Songs.Find(Id);
			if(song != null)
			{
				_db.Songs.Remove(song);
				_db.SaveChanges();
				return Ok("Succesfully Deleted");
			}
			else
				return BadRequest("Song not found");
		}
//		[Route("api/searchsong/{id}")]
//		[HttpGet]
//		public IHttpActionResult Search(int Id)
//		{
//			var song = _db.Songs.Find(Id);
//			if(song !=null)
//				return Ok(song);
//				else
//				return BadRequest("Not found");
//		}
		
		
	}
}