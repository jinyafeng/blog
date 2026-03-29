<template>
  <div class="admin-posts">
    <div class="header">
      <h2>文章管理</h2>
      <el-button type="primary" @click="$router.push('/admin/posts/edit')">写新文章</el-button>
    </div>
    <el-table :data="posts" border style="width: 100%">
      <el-table-column prop="id" label="ID" width="80"></el-table-column>
      <el-table-column prop="title" label="标题"></el-table-column>
      <el-table-column prop="category.name" label="分类" width="100"></el-table-column>
      <el-table-column label="状态" width="100">
        <template #default="scope">
          <el-tag :type="scope.row.isPublished ? 'success' : 'warning'">
            {{ scope.row.isPublished ? '已发布' : '草稿' }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="createdAt" label="创建时间" width="180">
        <template #default="scope">
          {{ new Date(scope.row.createdAt).toLocaleString() }}
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template #default="scope">
          <el-button size="small" @click="editPost(scope.row.id)">编辑</el-button>
          <el-button size="small" type="danger" @click="deletePost(scope.row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
const router = useRouter()
const posts = ref([])

const fetchPosts = async () => {
  const res = await fetch('api/BlogPosts?onlyPublished=false')
  posts.value = await res.json()
}

const editPost = (id) => {
  router.push(`/admin/posts/edit/${id}`)
}

const deletePost = async (id) => {
  try {
    await ElMessageBox.confirm('确定要删除这篇文章吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    await fetch(`api/BlogPosts/${id}`, {
      method: 'DELETE'
    })
    ElMessage.success('删除成功')
    fetchPosts()
  } catch {
    // 取消删除
  }
}

onMounted(() => {
  fetchPosts()
})
</script>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}
</style>
