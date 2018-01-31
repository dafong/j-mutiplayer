var info = wx.getSystemInfoSync()

export default {
    devicePixelRatio :window.devicePixelRatio,
    model : info.model,
    platform : info.platform,
    system : info.system,
    frustumsize : 8,
    ratio : window.innerWidth / window.innerHeight
}
