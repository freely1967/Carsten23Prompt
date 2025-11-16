PR ready for review: refactor/solid-decomposition -> main

Title:
Finalize SOLID refactors and prepare Customer data-holder conversion (review required)

Description (use as PR body):
See attached patch documentation `PATCH_1..PATCH_21_DOCUMENTATION.md` and the review plan `PATCH_21_REVIEW_CUSTOMER_REFACTOR.md`.

Summary:
- This branch modularizes discount logic, centralizes pricing, segregates payment interfaces, introduces a multiplier-capable discount engine, extracts loyalty and purchase services, and provides adapters for backwards compatibility. The high-risk final conversion of `Customer` into a pure data-holder is described in `PATCH_21_REVIEW_CUSTOMER_REFACTOR.md` and must be applied only after domain approval.

How to create PR (recommended):
- If you have GitHub CLI (`gh`) installed, run:

  gh pr create --title "Finalize SOLID refactors and prepare Customer data-holder conversion (review required)" \
    --body-file PR_DRAFT_REFRACTOR.md \
    --base main --head refactor/solid-decomposition --reviewer "backend-lead,qa-engineer"

- Or open a browser and create a PR using this URL (paste into browser address bar):

  https://github.com/freely1967/Carsten23Prompt/compare/main...refactor/solid-decomposition?expand=1

PR checklist (for reviewers):
- [ ] Verify unit and integration tests run successfully in CI
- [ ] Review `PATCH_21_REVIEW_CUSTOMER_REFACTOR.md` (final conversion plan) and approve or request changes
- [ ] Confirm VIP discount rules and VipLoyalty multiplier behavior
- [ ] Confirm exception vs logging policy for minor purchases

Requested reviewers:
- @backend-lead (replace with actual GitHub username)
- @qa-engineer
- @devops

Once reviewers approve, run the final apply_patch contained in `CUSTOMER_CONVERSION_PATCH.txt` (I prepared it in the repository). The final conversion is high-risk and must be applied only after sign-off.

Commands to run locally (Windows PowerShell):

cd c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood

# Run all tests
dotnet test ApocalypticFastFood.sln

# Create PR using gh (if available)
# gh pr create --title "Finalize SOLID refactors and prepare Customer data-holder conversion (review required)" --body-file PR_DRAFT_REFRACTOR.md --base main --head refactor/solid-decomposition

Notes:
- The branch `refactor/solid-decomposition` is up-to-date and contains the PATCH_*.md documentation files and the review document.
- I added `CUSTOMER_CONVERSION_PATCH.txt` — this contains the apply_patch-style diff to convert `Customer` to a data holder; do not apply until domain approval is granted.
