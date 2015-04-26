using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace Pacman.SoundHandler
{
    public class SoundHandler
    {
        public void OnNewGame(object sender, EventArgs e)
        {
            PlaySound(Pacman.SoundHandler.Properties.Resources.pacman_beginning);
        }

        public void OnGhostDie(object sender, EventArgs e)
        {
            PlaySound(Pacman.SoundHandler.Properties.Resources.pacman_eat_ghost);
        }

        public void OnEatItem(object sender, EventArgs e)
        {
            PlaySound(Pacman.SoundHandler.Properties.Resources.pacman_chomp);
        }

        private void PlaySound(Stream sound)
        {
            SoundPlayer player = new SoundPlayer(sound);
            player.Play();
        }
    }
}
