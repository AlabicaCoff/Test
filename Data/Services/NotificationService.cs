﻿using System.Diagnostics;
using Test.Models;

namespace Test.Data.Services
{
    public class NotificationService: INotificationService
    {
        private readonly TestDbContext _context;
        public NotificationService(TestDbContext context)
        {
            _context = context;
        }
        public void Add(Notification notification)
        {
            _context.Notifications.Add(notification);
        }

        public void Delete(Notification notification)
        {
            _context.Notifications.Remove(notification);
        }

        public IEnumerable<Notification> GetAll()
        {
            var result = _context.Notifications.ToList();
            return result;
        }

        public Notification GetById(int id)
        {
            var result = _context.Notifications.SingleOrDefault(n => n.Id == id);
            return result;
        }

        public void Update(int id, Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Send(string title, string postTitle, string link, string userId)
        {
            var notificationTitle = (title == "Congrats") ? "Congrats! you got a permission to join this post" + postTitle
                : "Sorry! you don't got a permission to join this post" + postTitle;
            var notification = new Notification()
            {
                Title = notificationTitle,
                Link = link,
                UserId = userId
            };
            Add(notification);
            Save();
        }
    }
}