using System;
using UnityEngine;
using Discord;

namespace Murgn
{
    public class DiscordRPC : MonoBehaviour
    {
        public Discord.Discord discord;
        
        private void Start()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            
            discord = new Discord.Discord(1032797452987793489, (UInt64)Discord.CreateFlags.Default);
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                // TODO: Replace this with map name!
                State = "In Map defaultMap",
                Details = "Playing with Powder",
                Timestamps =
                {
                    Start = secondsSinceEpoch
                },
                Assets =
                {
                    LargeImage = "largelogo",
                    LargeText = "PowderKeg",
                }
            };
            
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Debug.Log("Discord Status Set");
                }
                else
                {
                    Debug.LogError("Discord Status Failed");
                }
            });
        }

        private void Update() => discord.RunCallbacks();
        
        private void OnApplicationQuit() => discord.Dispose();
    }   
}