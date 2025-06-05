using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;

namespace MyPortfolioWebApp.Controllers
{
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Board
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            ViewData["Title"] = "서버에서 변경 가능"; 

            // 최종 단계
            var totalCount = _context.Board.Where(n => EF.Functions.Like(n.Title, $"%{search}%")).Count();
            var countList = 10;  // 한 페이지에 보여줄 뉴스 개수
            var totalPage = totalCount / countList;  // 한페이지당 개수로 나누면 전체페이지 수

            //  HACK : 게시판 페이지 중요로직. 남는 데이터도 한페이지를 차지해야함
            if (totalCount % countList > 0) totalPage++; // 최소 한페이지는 있다
            if (totalCount < page) page = totalPage;

            // 마지막 페이지 구하기
            var countPage = 10;  // 페이지를 표시할 최대 개수
            var startPage = ((page - 1) / countPage) * countPage + 1; // 시작 페이지
            var endPage = startPage + countPage - 1;

            // HACK :  나타낼 페이지수가 10이 안되면 페이지 수 조정.
            // 마지막 페이지까지 글이 12개면 1, 2페이지만 표시
            if (totalPage < endPage) endPage = totalPage;

            // 저장프로시저에 보낼 rowNum값, 시작번호랑 끝번호
            var startCount = ((page - 1) * countPage) + 1;  // 2페이지의 경우 11
            var endCount = startCount + countList - 1; // 2페이지의 경우 20

            // View로 넘기는 데이터, 페이징 숫자 컨트롤 사용
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.Search = search;

            // 자동 프로시저 호출
            var boards = await _context.Board.FromSql($"CALL Board_PagingBoard({startCount}, {endCount}, {search})").ToListAsync();
            return View(boards);
        }

        // GET: Board/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
            {
                return NotFound();
            }

            board.ReadCount++;
            _context.Board.Update(board);
            await _context.SaveChangesAsync();
            return View(board);
        }

        // GET: Board/Create
        public IActionResult Create()
        {
            var board = new Board
            {
                Email = User.Identity?.Name, // 현재 로그인한 사용자의 이메일
                Writer = "사용자", // 현재 로그인한 사용자의 이름
                PostDate = DateTime.Now, // 현재 날짜로 초기화
                ReadCount = 0 // 조회수 초기화
            };
            return View();
        }

        // POST: Board/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Title,Contents")] Board board)
        {
            if (ModelState.IsValid)
            {
                board.Email = User.Identity?.Name; // 현재 로그인한 사용자의 이메일
                board.Writer = "사용자"; // 현재 로그인한 사용자의 이름
                board.PostDate = DateTime.Now; // 현재 날짜로 초기화
                board.ReadCount = 0; // 조회수 초기화

                _context.Add(board);
                await _context.SaveChangesAsync();

                TempData["success"] = "게시글이 성공적으로 저장되었습니다.";
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Board/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        // POST: Board/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Title,Contents")] Board board)
        {
            if (id != board.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBoard = await _context.Board.FindAsync(id);
                    if (existingBoard == null)
                    {
                        return NotFound();
                    }
                    existingBoard.Email = User.Identity?.Name; // 현재 로그인한 사용자의 이메일
                    existingBoard.Title = board.Title; // 제목 업데이트
                    existingBoard.Contents = board.Contents; // 내용 업데이트

                    await _context.SaveChangesAsync();
                    TempData["success"] = "게시글이 성공적으로 수정되었습니다.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Board/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // POST: Board/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "게시글이 성공적으로 삭제되었습니다.";
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}
