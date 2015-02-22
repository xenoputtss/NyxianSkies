﻿var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var NyxianSkies;
(function (NyxianSkies) {
    var Preloader = (function (_super) {
        __extends(Preloader, _super);
        function Preloader() {
            _super.apply(this, arguments);
        }
        Preloader.prototype.preload = function () {
            this.preloadBar = this.add.sprite(640 - 200, 360 - 20, 'preloadBar');
            this.load.setPreloadSprite(this.preloadBar);

            // Load all of our assets here
            // Images
            this.load.image('spritesheet', 'assets/images/sheet.png');
            this.load.image('blueUISpriteSheet', 'assets/ui/blueSheet.png');
            this.load.image('greenUISpriteSheet', 'assets/ui/greenSheet.png');
            this.load.image('greyUISpriteSheet', 'assets/ui/greySheet.png');
            this.load.image('redUISpriteSheet', 'assets/ui/redSheet.png');
            this.load.image('yellowUISpriteSheet', 'assets/ui/yellowSheet.png');
            this.load.image('title', 'assets/images/title.png');
            this.load.image('blackBackground', 'assets/images/black.png');
            this.load.image('blueBackground', 'assets/images/blue.png');
            this.load.image('darkPurpleBackground', 'assets/images/darkPurple.png');
            this.load.image('purpleBackground', 'assets/images/purple.png');
            this.load.image('playerShip1_red', 'assets/images/playerShip1_red.png');

            this.load.spritesheet('blueUISpriteSheet-Button', 'assets/ui/blueSheet.png', 190, 49);

            // Audio
            this.load.audio('styx', 'assets/audio/styx.mp3');
        };

        Preloader.prototype.create = function () {
            var tween = this.add.tween(this.preloadBar).to({ alpha: 0 }, 1000, Phaser.Easing.Linear.None, true);
            tween.onComplete.add(this.startTitleScreen, this);
        };

        Preloader.prototype.startTitleScreen = function () {
            this.game.state.start('TitleScreen', true, false);
        };
        return Preloader;
    })(Phaser.State);
    NyxianSkies.Preloader = Preloader;
})(NyxianSkies || (NyxianSkies = {}));
//# sourceMappingURL=Preloader.js.map
