local env = os.getenv("ENV") or "DEV"
env = string.lower(env)
local str = string.format('conf.%s.config',env)
return require(str)
