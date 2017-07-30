define('companySpecificConfiguration', [], function () {
    var csc = {
        CompanyName: 'SogeCap',
        StrategyAssembly: 'EBranch.Strategy.SogeCap',
        GlobalExclusionFollowingSteps: 'Sorry, you cannot be granted an automatic offer as your insurance request needs to be studied by ARABIA. Please check back within the next 48 WORKING hours.',
		BrandModelListCorrelation: 'Type',
		ShowAds: false,
		PropertiesOnSignUp: 'location,salutation'
    };
    return csc;
});