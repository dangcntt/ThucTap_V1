<script>
export default {
  name: 'app',
  components: {
    
  },
  data() {
    return {
      data: [],
      url: `${process.env.VUE_APP_API_URL}filesminio/view`,
      apiView: `${process.env.VUE_APP_API_URL}filesminio/view/`,
    };
  },
  created() {
  },
  methods: {
    
  }
}
</script>
  <template>
        <div id="app">
          <iframe src="https://publuu.com/flip-book/348753/799398/page/1?embed" style="width: 100%; height:100vh" scrolling="no" frameborder="0" allowfullscreen="" allow="clipboard-write" class="publuuflip"></iframe>
            <!-- <div class="book-weight">
                <Book>
                  <div class="page cover">
                    <article class="content">
                      <img src="@/assets/images/sach/BIA.png" style="width: 100%; height: 100%; padding: 10px;"/>
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (1).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (2).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (3).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (4).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (5).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (6).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (7).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (8).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (9).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (10).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (11).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (12).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (13).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (14).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (15).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (16).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (17).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (18).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (19).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (20).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (21).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (22).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (23).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (24).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (25).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (26).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (27).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (28).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (29).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (30).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page">
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (31).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                    <article class="content">
                        <img src="@/assets/images/sach/SACHHOA-TRANG (32).jpg" alt="" style="width: 100%; height: 100%;">
                    </article>
                  </div>
                  <div class="page back-back"></div>
                </Book>

            </div> -->
        </div>
  </template>
  
<style scoped>
#root{
  background-color: #fff;
  background-image: none !important;
}
.loader{
  background-color: #fff !important;
}

body.light .header__border{
  background-color: rgb(216 38 110) !important;
  opacity: 1 !important;
}
</style>
  