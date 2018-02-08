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
    gravity: 9.8,
    speedY:6,
    speedZ:15
}
