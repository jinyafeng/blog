<template>
  <div class="edit-post">
    <h2>{{ postId ? '编辑文章' : '写新文章' }}</h2>
    <el-form :model="post" label-width="100px" style="max-width: 1200px">
      <el-form-item label="标题">
        <el-input v-model="post.title" placeholder="请输入文章标题"></el-input>
      </el-form-item>
      <el-form-item label="Slug">
        <el-input v-model="post.slug" placeholder="url地址，如my-first-post"></el-input>
      </el-form-item>
      <el-form-item label="分类">
        <el-select v-model="post.categoryId" placeholder="请选择分类">
          <el-option v-for="category in categories" :key="category.id" :label="category.name" :value="category.id"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="标签">
        <el-input v-model="tagsStr" placeholder="多个标签用逗号分隔"></el-input>
      </el-form-item>
      <el-form-item label="摘要">
        <el-input type="textarea" v-model="post.excerpt" :rows="3" placeholder="选填，文章摘要"></el-input>
      </el-form-item>
      <el-form-item label="内容">
        <div class="editor-container">
          <Editor v-model="post.content" :plugins="plugins" />
        </div>
      </el-form-item>
      <el-form-item label="发布">
        <el-switch v-model="post.isPublished" active-text="立即发布"></el-switch>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="savePost">保存</el-button>
        <el-button @click="$router.back()">取消</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Editor } from '@bytemd/vue-next'
import gfm from '@bytemd/plugin-gfm'
import 'bytemd/dist/index.css'

const route = useRoute()
const router = useRouter()
const postId = route.params.id
const plugins = [gfm()]
const categories = ref([])
const tagsStr = ref('')
const post = ref({
  title: '',
  slug: '',
  content: '',
  excerpt: '',
  categoryId: 1,
  tags: [],
  isPublished: false
})

const fetchCategories = async () => {
  const res = await fetch('api/Categories')
  categories.value = await res.json()
}

const fetchPost = async () => {
  const res = await fetch(`api/BlogPosts/${postId}`)
  post.value = await res.json()
  tagsStr.value = post.value.tags.join(',')
}

const savePost = async () => {
  post.value.tags = tagsStr.value.split(',').map(t => t.trim()).filter(t => t)
  if (!post.value.title || !post.value.slug || !post.value.content) {
    ElMessage.warning('标题、Slug和内容不能为空')
    return
  }
  let url = 'api/BlogPosts'
  let method = 'POST'
  if (postId) {
    url += `/${postId}`
    method = 'PUT'
  }
  await fetch(url, {
    method,
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(post.value)
  })
  ElMessage.success('保存成功')
  router.push('/admin/posts')
}

onMounted(() => {
  fetchCategories()
  if (postId) {
    fetchPost()
  }
})
</script>

<style scoped>
.edit-post {
  padding: 20px;
}
.editor-container {
  border: 1px solid #dcdfe6;
  border-radius: 4px;
}
:deep(.bytemd) {
  height: 600px;
}
</style>
