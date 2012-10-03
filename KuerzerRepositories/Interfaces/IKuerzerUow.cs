namespace KuerzerRepositories.Interfaces
{
    /// <summary>
    /// Interface for the Code Camper "Unit of Work"
    /// </summary>
    public interface IKuerzerUow
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        IUserProfileRepository UserProfiles { get; }
		ILinkRepository Links { get; }
		IUserApplicationRepository UserApplications { get; }
     //   IRepository<Room> Rooms { get; }
        //ISessionsRepository Sessions { get; }
       // IRepository<TimeSlot> TimeSlots { get; }
   //     IRepository<Track> Tracks { get; }
    }
}