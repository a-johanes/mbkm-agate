using System.Collections.Generic;
using System.Linq;
using Player.Command;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const int MaxUndoCommandCount = 100;
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public PlayerShooting playerShooting;

    //Queue untuk menyimpan list command
    private readonly List<Command> _commands = new List<Command>();

    private void Update()
    {
        //Mengahndle shoot
        var shootCommand = InputShootHandling();
        shootCommand?.Execute();
    }

    private void FixedUpdate()
    {
        //Menghandle input movement
        var moveCommand = InputMovementHandling();
        if (moveCommand != null)
        {
            _commands.Add(moveCommand);

            moveCommand.Execute();
        }

        if (_commands.Count() >= 2 * MaxUndoCommandCount)
        {
            _commands.RemoveRange(0,_commands.Count() - MaxUndoCommandCount);
        }
    }

    private Command InputMovementHandling()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            print("z");
            //Undo movement
            return Undo();
        }

        if (playerHealth.currentHealth <= 0) return null;

        float v;
        var h = v = 0;
        //Check jika movement sesuai dengan key nya
        if (Input.GetKey(KeyCode.D)) h += 1;

        if (Input.GetKey(KeyCode.A)) h -= 1;

        if (Input.GetKey(KeyCode.W)) v += 1;

        if (Input.GetKey(KeyCode.S)) v -= 1;

        return new MoveCommand(playerMovement, h, v);
    }

    private Command Undo()
    {
        //Jika Queue command tidak kosong, lakukan perintah undo
        if (_commands.Count > 0)
        {
            var undoCommand = _commands.Last();
            _commands.RemoveAt(_commands.Count() - 1);
            undoCommand.UnExecute();
        }

        return null;
    }

    private Command InputShootHandling()
    {
        //Jika menembak trigger shoot command
        if (Input.GetButton("Fire1") && playerShooting.Timer >= playerShooting.timeBetweenBullets &&
            Time.timeScale != 0 && playerHealth.currentHealth > 0)
            return new ShootCommand(playerShooting);

        return null;
    }
}