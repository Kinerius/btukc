using System.Collections.Generic;
using UnityEngine;

namespace Tags
{
    public class TagService
    {
        private readonly HashSet<int> tags = new ();
        private readonly Dictionary<int, int> tagStacks = new ();

        // We probably want to throw an event here
        public void ApplyTag(int effectTag)
        {
            tags.Add(effectTag);
        }

        public void ApplyStack(int effectTag, int amount = 1)
        {
            if (!HasTag(effectTag))
                ApplyTag(effectTag);

            tagStacks.TryAdd(effectTag, 0);
            tagStacks[effectTag] += amount;
        }

        public void RemoveStack(int effectTag)
        {
            if (!HasTag(effectTag)) return;

            tagStacks[effectTag]--;

            if (tagStacks[effectTag] <= 0)
            {
                RemoveTag(effectTag);
            }
        }

        public int GetStack(int effectTag) =>
            !HasTag(effectTag) ? 0 : tagStacks[effectTag];

        public bool HasTag(int effectTag) =>
            tags.Contains(effectTag);

        // We probably want to throw an event here
        public void RemoveTag(int effectTag)
        {
            tags.Remove(effectTag);

            if (tagStacks.ContainsKey(effectTag))
                tagStacks.Remove(effectTag);
        }
    }
}
