using Inputs;
using UnityEngine;
using UnityEngine.UI;

public class Installer : MonoBehaviour
{
	[SerializeField] private Vector2Int _gridSize = new Vector2Int(20, 10);
	[SerializeField] private FigureVariants _figureVariants;
	[SerializeField] private GameObject _blockPrefab;
	[SerializeField] private Image _UIBlockPrefab;
	[SerializeField] private RowGOPresenter _rowPrefab;
	[SerializeField] private int _blockPoolPreloadCount = 20;
	[SerializeField] private SimpleInputProvider _simpleInputProvider;

	[Space]
	[SerializeField] private Score _score;

	[SerializeField] private Game _game;

	[Space]
	[SerializeField] private GridGOPresenter _gridPresenter;

	[SerializeField] private FigureGOPresenter _figurePresenter;
	[SerializeField] private ScorePresenter _scorePresenter;
	[SerializeField] private PausePresenter _pausePresenter;
	[SerializeField] private MenuPresenter _menuPresenter;
	[SerializeField] private NextFigurePresenter _nextFigurePresenter;

	private void Awake()
	{
		GameGrid.Init(_gridSize);
		_game.Init(new Randomizer(_figureVariants), _simpleInputProvider);
		GOPool blockPool = new GOPool(_blockPrefab);

		ComponentPool<RowGOPresenter> rowPool = new ComponentPool<RowGOPresenter>(_rowPrefab);
		ComponentPool<Image> imagePool = new ComponentPool<Image>(_UIBlockPrefab);
		blockPool.Preload(_blockPoolPreloadCount);
		rowPool.Preload(GameGrid.Size.y);

		PauseController pauseController = new PauseController(_simpleInputProvider, _game);
		Menu menu = new Menu(pauseController, _game, _score);

		_menuPresenter.Inject(menu);
		_pausePresenter.Inject(pauseController);
		_scorePresenter.Inject(_score);
		_nextFigurePresenter.Inject(_game);
		_gridPresenter.Inject(_game);
		_figurePresenter.Inject(_game);

		_nextFigurePresenter.SetPool(imagePool);
		_gridPresenter.SetPool(blockPool, rowPool);
		_figurePresenter.SetPool(blockPool);
	}
}