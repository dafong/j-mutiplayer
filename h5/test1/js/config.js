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
    gravity: 9.8,
    speedY:12,
    speedZ:15,
    design:{x : 414 ,y : 736}
}
