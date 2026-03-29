<template>
  <el-container style="height: 100vh">
    <el-aside width="200px">
      <el-menu
        :router="true"
        default-active="/admin/posts"
        class="el-menu-vertical-demo"
        style="height: 100%"
      >
        <el-menu-item index="/admin/posts">
          <i class="el-icon-document"></i>
          <span>文章管理</span>
        </el-menu-item>
        <el-menu-item index="/admin/posts/edit">
          <i class="el-icon-edit"></i>
          <span>写文章</span>
        </el-menu-item>
      </el-menu>
    </el-aside>
    <el-container>
      <el-header style="text-align: right; font-size: 14px">
        <el-button type="text" @click="$router.push('/')">返回前台</el-button>
      </el-header>
      <el-main>
        <router-view></router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup>
</script>

<style scoped>
.el-header {
  background-color: #f5f7fa;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding-right: 20px;
  border-bottom: 1px solid #e4e7ed;
}
.el-aside {
  background-color: #304156;
}
.el-menu-vertical-demo {
  border-right: none;
}
</style>
