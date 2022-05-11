namespace AlsTools.Core.ValueObjects
{
    public class Locator
    {
        /// <summary>
        /// Locator Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The time point where the location is set
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Locator name (displayed in arrangement window)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Locator notes
        /// </summary>
        public string Annotation { get; set; }

        /// <summary>
        /// Whether or not the locator is set as Song Start
        /// </summary>
        public bool IsSongStart { get; set; }
    }
}