﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepositoryLayer;
using ServiceLayer.Service.Interface;
using DomainLayer.Data;
using RepositoryLayer.Repository.UnitOfWork;

namespace ServiceLayer.Service.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly IUserService _userService;

        public CommentService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<ActionResult<CommentModel>> AddComment(EventViewModel bookEvent)
        {
            var comment = new CommentModel()
            {
                FirstName = _userService.FirsTName(),
                LastName = _userService.LastName(),
                Text = bookEvent.Comment.Text,
                UserId = _userService.GetEmail(),
                EventId = bookEvent.EventDetails.Id,
                TimeStamp = DateTime.Now
            };

            var newComment = new CommentEntity()
            {
                FirstName = comment.FirstName,
                LastName = comment.LastName,
                Text = comment.Text,
                UserId = comment.UserId,
                EventId = comment.EventId,
                TimeStamp = DateTime.Now
            };
            await _unitOfWork.CommentRepository.Add(newComment);

            return comment;
        }

        public async Task<List<CommentModel>> GetComments(int id)
        {
            var comments = new List<CommentModel>();
            var allComments = await _unitOfWork.CommentRepository.All();

            if (allComments?.Any() == true)
            {
                foreach (var comment in allComments)
                {
                    if (comment.EventId == id)
                    {
                        comments.Add(new CommentModel()
                        {
                            FirstName = comment.FirstName,
                            LastName = comment.LastName,
                            Text = comment.Text,
                            TimeStamp = comment.TimeStamp
                        });
                    }
                }

                comments = comments.OrderByDescending(x => x.TimeStamp.TimeOfDay)
                                            .ThenBy(x => x.TimeStamp.Date)
                                            .ThenBy(x => x.TimeStamp.Year).ToList();
            }
            return comments;
        }

    }
}
