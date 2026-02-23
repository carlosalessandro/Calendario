# Contributing to ProCalendar.Maui

First off, thank you for considering contributing to ProCalendar.Maui! 🎉

## 🤝 How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check existing issues. When creating a bug report, include:

- **Clear title and description**
- **Steps to reproduce**
- **Expected vs actual behavior**
- **Screenshots** (if applicable)
- **Environment details** (OS, .NET version, device)

### Suggesting Features

Feature suggestions are welcome! Please:

- **Check existing feature requests** first
- **Provide clear use case** and rationale
- **Consider implementation complexity**
- **Be open to discussion**

### Pull Requests

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/amazing-feature`)
3. **Make your changes**
4. **Add tests** for new functionality
5. **Ensure all tests pass**
6. **Update documentation**
7. **Commit with clear messages** (`git commit -m 'Add amazing feature'`)
8. **Push to branch** (`git push origin feature/amazing-feature`)
9. **Open a Pull Request**

## 📝 Code Style

### C# Guidelines

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable names
- Add XML documentation for public APIs
- Keep methods focused and small
- Use async/await properly

### Example:

```csharp
/// <summary>
/// Calculates the days to display for a given month.
/// </summary>
/// <param name="year">The year.</param>
/// <param name="month">The month (1-12).</param>
/// <param name="culture">The culture for week calculations.</param>
/// <returns>List of calendar days.</returns>
public List<CalendarDay> GetDaysForMonth(int year, int month, CultureInfo culture)
{
    // Implementation
}
```

## 🧪 Testing

- Write unit tests for new features
- Ensure existing tests pass
- Aim for 80%+ code coverage
- Test on multiple platforms

```bash
# Run tests
dotnet test

# With coverage
dotnet test /p:CollectCoverage=true
```

## 📚 Documentation

- Update README.md for new features
- Add examples to EXAMPLES.md
- Update ARCHITECTURE.md for architectural changes
- Include XML documentation comments

## 🔄 Development Workflow

1. **Setup development environment**
   ```bash
   git clone https://github.com/yourorg/procalendar.git
   cd procalendar
   dotnet restore
   ```

2. **Create feature branch**
   ```bash
   git checkout -b feature/my-feature
   ```

3. **Make changes and test**
   ```bash
   dotnet build
   dotnet test
   ```

4. **Commit and push**
   ```bash
   git add .
   git commit -m "Description of changes"
   git push origin feature/my-feature
   ```

5. **Create Pull Request**

## 🎯 Commit Message Guidelines

Use conventional commits:

- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `style:` Code style changes (formatting)
- `refactor:` Code refactoring
- `test:` Adding tests
- `chore:` Maintenance tasks

Example:
```
feat: add drag and drop event rescheduling

- Implement drag gesture recognition
- Add visual feedback during drag
- Update event position on drop
- Add unit tests

Closes #123
```

## 🏆 Recognition

Contributors will be:
- Listed in CONTRIBUTORS.md
- Mentioned in release notes
- Eligible for swag (future)

## 📞 Questions?

- Open a [GitHub Discussion](https://github.com/yourorg/procalendar/discussions)
- Email: contribute@procalendar.dev

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for making ProCalendar.Maui better! 🚀
