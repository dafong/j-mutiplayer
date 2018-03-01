var info = wx.getSystemInfoSync()

export default {
    devicePixelRatio :window.devicePixelRatio,
    model : info.model,
    platform : info.platform,
    system : info.system,
    frustumsize : 8,
    ratio : window.innerWidth / window.innerHeight,
    width : window.innerWidth,
    height: window.innerHeight,
    floor_height:2,
    floor_radius:0.5,
    gravity: 144,
    baseY :30,
    speedY:6,
    speedZ:25,
    design:{x : 414 ,y : 736},
    ajax:'http://127.0.0.1:8000',
    socket:'ws://127.0.0.1:4000/ws',
    noop:function(){}
}
// velocityYIncrement: 15,
// velocityY: 135,
// velocityZIncrement: 70
