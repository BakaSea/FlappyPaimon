using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum GameStatus {
    START = 0,
    RUN = 1,
    OVER = 2,
    WIN = 3,
    WINOVER = 4
}

class GameController {

    public static GameStatus status = GameStatus.START;
    public static int score = 0;
    public static float flySpeed = 2.0f;
    public static float genObjectTime = 2.5f;

    public static void start() {
        score = 0;
        status = GameStatus.START;
    }

    public static void run() {
        status = GameStatus.RUN;
    }

    public static void over() {
        status = GameStatus.OVER;
    }

    public static void win() {
        status = GameStatus.WIN;
    }

    public static void winover() {
        status = GameStatus.WINOVER;
    }

}
