
// This class contains metadata for your submission. It plugs into some of our
// grading tools to extract your game/team details. Ensure all Gradescope tests
// pass when submitting, as these do some basic checks of this file.
public static class SubmissionInfo
{
    // TASK: Fill out all team + team member details below by replacing the
    // content of the strings. Also ensure you read the specification carefully
    // for extra details related to use of this file.

    // URL to your group's project 2 repository on GitHub.
    public static readonly string RepoURL = "https://github.com/COMP30019/project-2-team-orange";
    
    // Come up with a team name below (plain text, no more than 50 chars).
    public static readonly string TeamName = "Team Orange";
    
    // List every team member below. Ensure student names/emails match official
    // UniMelb records exactly (e.g. avoid nicknames or aliases).
    public static readonly TeamMember[] Team = new[]
    {
        new TeamMember("Yi Zhao", "zhaoyz6@student.unimelb.edu.au"),
        new TeamMember("William Sutherland", "wfsutherland@student.unimelb.edu.au"),
        new TeamMember("Pratika Dlima", "pdlima@student.unimelb.edu.au"),
        // Remove the following line if you have a group of 3
        new TeamMember("Shanaia Chen", "chenso@student.unimelb.edu.au"), 
    };

    // This may be a "working title" to begin with, but ensure it is final by
    // the video milestone deadline (plain text, no more than 50 chars).
    public static readonly string GameName = "Fly-by"; 

    // Write a brief blurb of your game, no more than 200 words. Again, ensure
    // this is final by the video milestone deadline.
    public static readonly string GameBlurb = "Fly-by is a game that allows players to play as a plane 'flying by' different obstacles (bird, other planes, hot air balloons) in the sky through keyboard controls (Up/down/left/right). The aim of the game is to get as much points, so there is no win condition. However, as you progress and pass certain score thresholds, the plane gets faster and flies through different places which have different themes and new obstacles. Additionally, there are power-ups that allow you to be shielded or speed up for a limited time.";
    
    // By the gameplay video milestone deadline this should be a direct link
    // to a YouTube video upload containing your video. Ensure "Made for kids"
    // is turned off in the video settings. 
    public static readonly string GameplayVideo = "https://www.youtube.com/watch?v=xf4h4RGnftY";
    
    // No more info to fill out!
    // Please don't modify anything below here.
    public readonly struct TeamMember
    {
        public TeamMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}
