namespace Player.Command
{
    public abstract class Command
    {
        public abstract void Execute();
        public abstract void UnExecute();
    }
    
    public class MoveCommand : Command
    {
        PlayerMovement playerMovement;
        float h, v;

        public MoveCommand(PlayerMovement playerMovement, float h, float v)
        {
            this.playerMovement = playerMovement;
            this.h = h;
            this.v = v;
        }

        //Trigger perintah movement
        public override void Execute()
        {
            playerMovement.Move(h, v);
            //Menganimasikan player
            playerMovement.Animating(h, v);
        }

        public override void UnExecute()
        {
            //Invers arah dari movement player
            playerMovement.Move(- h, - v);
            //Menganimasikan player
            playerMovement.Animating(h, v);
        }
    }
    
    public class ShootCommand : Command
    {

        PlayerShooting playerShooting;

        public ShootCommand(PlayerShooting playerShooting)
        {
            this.playerShooting = playerShooting;
        }

        public override void Execute()
        {
            //Player menembak
            playerShooting.Shoot();
        }

        public override void UnExecute()
        {
        
        }
    }
}
