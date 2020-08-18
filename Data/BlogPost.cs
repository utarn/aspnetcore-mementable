using System;
using MementoExtension.Data;
using MementoExtension.Interfaces;

namespace aspnetcore_mementable.Data
{
    public class BlogPost : Mementable
    {
        public int BlogPostId { get; set; }
        public string Content { get; set; }
        public DateTime LastUpdated { get; set; }

        public State CreateState()
        {
            return new State(this);
        }

        public void RestoreState(State memento)
        {
            var newState = memento.GetObject() as BlogPost;
            this.BlogPostId = newState.BlogPostId;
            this.Content = newState.Content;
            this.LastUpdated = newState.LastUpdated;
        }
    }
}