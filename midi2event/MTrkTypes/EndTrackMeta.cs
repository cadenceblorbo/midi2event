namespace midi2event
{
    internal class EndTrackMeta: MTrkEvent {

        public EndTrackMeta(uint delta){
            this.Delta = delta;
        }
     }
}
